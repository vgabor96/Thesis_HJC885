#include "cuda_runtime.h"
#include "device_launch_parameters.h"
#include <math.h>  
#include <stdio.h>
#include <stdlib.h> 
#include <time.h> 
#include "Timing.h"
#include <opencv2\core\mat.hpp>
#include <opencv2\core\cuda.inl.hpp>
#include <iostream>
#include <opencv2\highgui.hpp>
#include <opencv2\imgproc.hpp>
using namespace std;
using namespace cv;


const int N = 768;
const int M = 1024;

const int NM = N * M;

//Konstansok
const int RND_Min = 0;
const int RND_Max = 255;
const int MaxThreads = 1024;

__device__ const int dev_N = 768;
__device__ const int dev_M = 1024;
__device__ const int dev_NM = dev_N * dev_M;

__device__ const int dev_MaxThreads = 1024;
__device__ const int dev_Max = 255;

//Mérési eszközök:

cudaEvent_t start;
cudaEvent_t stop;

//RGB csatornák (eredeti kép):
float channel_r[N][M];
float channel_g[N][M];
float channel_b[N][M];

//Módosított képek:
int grayScale[N][M];     // fekete-fehér kép

int forMinMaxSearch[NM]; // vektor a Minimum és Maximum kereséshez

int histogram[N][M];     // histogram korrekció

int noNoise[N][M];		 // Gauss után

int blackAndWhite[N][M]; // csak 0 és 255 értékû pixelek

int valueMatrix[N][M];	 // csak -3 és 255 értékû pixelek

//Segéd változók:
int globalMin[1];		  // globális Minimum
int globalMax[1];	      // globális Maximum

int darkPixelCounter[1] = { 0 };

int avgPixelColor[1] = { 0 };
int avgPixelColorCounter[1] = { 0 };

//GAUSS Mátrixok:
int GaussSize = 3;
int GaussValue[1] = { 0 };
int GaussMatrix[3][3] =
{
	{1, 2, 1},
	{2, 4, 2},
	{1, 2, 1}
};


//***********************************************************************************//

//GPU - RGB csatornák (eredeti kép):
__device__ float dev_channel_r[N][M];
__device__ float dev_channel_g[N][M];
__device__ float dev_channel_b[N][M];

//GPU - Módosított képek:
__device__ int dev_grayScale[N][M];     // fekete-fehér kép

__device__ int dev_forMinMaxSearch[NM]; // vektor a Minimum és Maximum kereséshez

__device__ int dev_histogram[N][M];     // histogram korrekció

__device__ int dev_noNoise[N][M];

__device__ int dev_blackAndWhite[N][M];

__device__ int dev_valueMatrix[N][M];


int res_grayScale[N][M];

int res_forMinMaxSearch[NM];

int res_histogram[N][M];

int res_noNoise[N][M];

int res_blackAndWhite[N][M];

int res_valueMatrix[N][M];

//GPU - Segéd változók:
__device__ int localMin[(NM + 1) / 2];
__device__ int localMax[(NM + 1) / 2];

__device__ int dev_globalMin[1];		  // globális Minimum
__device__ int dev_globalMax[1];	      // globális Maximum

__device__ int dev_darkPixelCounter[1] = { 0 };

__device__ int dev_avgPixelColor[1] = { 0 };
__device__ int dev_avgPixelColorCounter[1] = { 0 };


int res_globalMin[1];		  // globális Minimum
int res_globalMax[1];

int res_darkPixelCounter[1] = { 0 };

int res_avgPixelColor[1] = { 0 };

//GPU - GAUSS Mátrixok:
__device__ int dev_GaussSize = 3;
__device__ int dev_GaussValue[1] = { 0 };
__device__ int dev_GaussMatrix[3][3] =
{
	{1, 2, 1},
	{2, 4, 2},
	{1, 2, 1}
};

int res_GaussValue[1] = { 0 };

//********************************************************************************************************************//
//Random kép generálása:

void RandomPicture_CPU()
{
	srand(time(NULL));
	for (int i = 0; i < N; i++)
	{
		for (int j = 0; j < M; j++)
		{
			channel_r[i][j] = (rand() % (RND_Min - RND_Max + 1)) + RND_Min;
			channel_g[i][j] = (rand() % (RND_Min - RND_Max + 1)) + RND_Min;
			channel_b[i][j] = (rand() % (RND_Min - RND_Max + 1)) + RND_Min;
		}
	}
}

//********************************************************************************************************************//
//Kép beolvasása:


//********************************************************************************************************************//
//Fekete fehér:

__global__ void GrayScale()
{
	int j = threadIdx.x;

	if (dev_M <= dev_MaxThreads)
	{
		if (j < dev_M)
		{
			for (int i = 0; i < dev_N; i++)
			{
				dev_grayScale[i][j] = __float2int_rn((dev_channel_r[i][j] + dev_channel_g[i][j] + dev_channel_b[i][j]) / 3);
			}
		}
	}
	else
	{
		int run = __float2int_ru(dev_M / dev_MaxThreads);
		for (int block_i = 0; block_i < run; block_i++)
		{
			int modifier = block_i * dev_MaxThreads;

			for (int i = 0; i < N; i++)
			{
				if ((j + modifier) < dev_M)
				{
					dev_grayScale[i][j + modifier] = __float2int_rn((dev_channel_r[i][j + modifier] + dev_channel_g[i][j + modifier] + dev_channel_b[i][j + modifier]) / 3);
				}
			}
		}
	}
}

void GrayScale_CPU()
{
	for (int i = 0; i < N; i++)
	{
		for (int j = 0; j < M; j++)
		{
			grayScale[i][j] = round((channel_r[i][j] + channel_g[i][j] + channel_b[i][j]) / 3);
		}
	}
}

//********************************************************************************************************************//
//2D tömb --> 1D vektor konvertálás:

__global__ void ConvertArrayToVector()
{
	int j = threadIdx.x;

	if (dev_M <= dev_MaxThreads)
	{
		if (j < dev_M)
		{
			for (int i = 0; i < dev_N; i++)
			{
				int idx = (i * dev_M) + j;
				dev_forMinMaxSearch[idx] = dev_grayScale[i][j];
			}
		}
	}
	else
	{
		int run = __float2int_ru(dev_M / dev_MaxThreads);
		for (int block_i = 0; block_i < run; block_i++)
		{
			int modifier = block_i * dev_MaxThreads;
			for (int i = 0; i < dev_N; i++)
			{
				if (j + modifier < dev_M)
				{
					int idx = (i * dev_M) + j + modifier;
					dev_forMinMaxSearch[idx] = dev_grayScale[i][j + modifier];
				}
			}
		}
	}
}

//Minimum kiválasztás:

__global__ void MinSearch()
{
	int blockSize = dev_NM;

	int num_1_idx = threadIdx.x * 2;
	int num_2_idx = threadIdx.x * 2 + 1;
	int localMinValue = -1;

	if (dev_M <= dev_MaxThreads)
	{
		if (threadIdx.x < (dev_NM + 1) / 2)
		{
			while (blockSize > 0)
			{
				if (blockSize == dev_NM)
				{
					if (num_1_idx < blockSize && num_2_idx < blockSize)
					{
						localMinValue = dev_forMinMaxSearch[num_1_idx] < dev_forMinMaxSearch[num_2_idx] ? dev_forMinMaxSearch[num_1_idx] : dev_forMinMaxSearch[num_2_idx];
					}

					if (num_1_idx < blockSize && blockSize <= num_2_idx)
					{
						localMinValue = dev_forMinMaxSearch[num_1_idx];
					}
					__syncthreads();

					localMin[threadIdx.x] = localMinValue;
					__syncthreads();
				}
				else
				{
					if (num_1_idx < blockSize && num_2_idx < blockSize)
					{
						localMinValue = localMin[num_1_idx] < localMin[num_2_idx] ? localMin[num_1_idx] : localMin[num_2_idx];
					}

					if (num_1_idx < blockSize && blockSize <= num_2_idx)
					{
						localMinValue = localMin[num_1_idx];
					}
					__syncthreads();

					localMin[threadIdx.x] = localMinValue;
					__syncthreads();
				}

				if (blockSize % 2 == 1 && blockSize != 1)
				{
					blockSize++;
				}

				blockSize = blockSize / 2;
			}
		}
	}
	else
	{
		int run = __float2int_ru(dev_M / dev_MaxThreads);
		for (int block_i = 0; block_i < run; block_i++)
		{
			int modifier = block_i * dev_MaxThreads;
			num_1_idx = (threadIdx.x + modifier) * 2;
			num_2_idx = (threadIdx.x + modifier) * 2 + 1;
			while (blockSize > 0)
			{
				if (blockSize == dev_NM)
				{
					if (num_1_idx < blockSize && num_2_idx < blockSize)
					{
						localMinValue = dev_forMinMaxSearch[num_1_idx] < dev_forMinMaxSearch[num_2_idx] ? dev_forMinMaxSearch[num_1_idx] : dev_forMinMaxSearch[num_2_idx];
					}

					if (num_1_idx < blockSize && blockSize <= num_2_idx)
					{
						localMinValue = dev_forMinMaxSearch[num_1_idx];
					}
					__syncthreads();

					localMin[threadIdx.x] = localMinValue;
					__syncthreads();
				}
				else
				{
					if (num_1_idx < blockSize && num_2_idx < blockSize)
					{
						localMinValue = localMin[num_1_idx] < localMin[num_2_idx] ? localMin[num_1_idx] : localMin[num_2_idx];
					}

					if (num_1_idx < blockSize && blockSize <= num_2_idx)
					{
						localMinValue = localMin[num_1_idx];
					}
					__syncthreads();

					localMin[threadIdx.x] = localMinValue;
					__syncthreads();
				}

				if (blockSize % 2 == 1 && blockSize != 1)
				{
					blockSize++;
				}

				blockSize = blockSize / 2;
			}
		}
	}

	if (threadIdx.x == 0)
	{
		dev_globalMin[0] = localMin[0];
	}
}

void MinSearch_CPU()
{
	globalMin[0] = grayScale[0][0];

	for (int i = 0; i < N; i++)
	{
		for (int j = 0; j < M; j++)
		{
			if (grayScale[i][j] < globalMin[0])
			{
				globalMin[0] = grayScale[i][j];
			}
		}
	}
}

//Maximum kiválasztás:

__global__ void MaxSearch()
{
	int blockSize = dev_NM;

	int num_1_idx = threadIdx.x * 2;
	int num_2_idx = threadIdx.x * 2 + 1;
	int localMaxValue = -1;

	if (dev_M <= dev_MaxThreads)
	{
		if (threadIdx.x < (dev_NM + 1) / 2)
		{
			while (blockSize > 0)
			{
				if (blockSize == dev_NM)
				{
					if (num_1_idx < blockSize && num_2_idx < blockSize)
					{
						localMaxValue = dev_forMinMaxSearch[num_1_idx] > dev_forMinMaxSearch[num_2_idx] ? dev_forMinMaxSearch[num_1_idx] : dev_forMinMaxSearch[num_2_idx];
					}

					if (num_1_idx < blockSize && blockSize <= num_2_idx)
					{
						localMaxValue = dev_forMinMaxSearch[num_1_idx];
					}
					__syncthreads();

					localMax[threadIdx.x] = localMaxValue;
					__syncthreads();
				}
				else
				{
					if (num_1_idx < blockSize && num_2_idx < blockSize)
					{
						localMaxValue = localMax[num_1_idx] > localMax[num_2_idx] ? localMax[num_1_idx] : localMax[num_2_idx];
					}

					if (num_1_idx < blockSize && blockSize <= num_2_idx)
					{
						localMaxValue = localMax[num_1_idx];
					}
					__syncthreads();

					localMax[threadIdx.x] = localMaxValue;
					__syncthreads();
				}

				if (blockSize % 2 == 1 && blockSize != 1)
				{
					blockSize++;
				}

				blockSize = blockSize / 2;
			}
		}
	}
	else
	{
		int run = __float2int_ru(dev_M / dev_MaxThreads);
		for (int block_i = 0; block_i < run; block_i++)
		{
			int modifier = block_i * dev_MaxThreads;
			num_1_idx = (threadIdx.x + modifier) * 2;
			num_2_idx = (threadIdx.x + modifier) * 2 + 1;

			while (blockSize > 0)
			{
				if (blockSize == dev_NM)
				{
					if (num_1_idx < blockSize && num_2_idx < blockSize)
					{
						localMaxValue = dev_forMinMaxSearch[num_1_idx] > dev_forMinMaxSearch[num_2_idx] ? dev_forMinMaxSearch[num_1_idx] : dev_forMinMaxSearch[num_2_idx];
					}

					if (num_1_idx < blockSize && blockSize <= num_2_idx)
					{
						localMaxValue = dev_forMinMaxSearch[num_1_idx];
					}
					__syncthreads();

					localMax[threadIdx.x] = localMaxValue;
					__syncthreads();
				}
				else
				{
					if (num_1_idx < blockSize && num_2_idx < blockSize)
					{
						localMaxValue = localMax[num_1_idx] > localMax[num_2_idx] ? localMax[num_1_idx] : localMax[num_2_idx];
					}

					if (num_1_idx < blockSize && blockSize <= num_2_idx)
					{
						localMaxValue = localMax[num_1_idx];
					}
					__syncthreads();

					localMax[threadIdx.x] = localMaxValue;
					__syncthreads();
				}

				if (blockSize % 2 == 1 && blockSize != 1)
				{
					blockSize++;
				}

				blockSize = blockSize / 2;
			}
		}
	}

	if (threadIdx.x == 0)
	{
		dev_globalMax[0] = localMax[0];
	}
}

void MaxSearch_CPU()
{
	globalMax[0] = grayScale[0][0];

	for (int i = 0; i < N; i++)
	{
		for (int j = 0; j < M; j++)
		{
			if (globalMax[0] < grayScale[i][j])
			{
				globalMax[0] = grayScale[i][j];
			}
		}
	}
}

//Histogram széthúzás:

__global__ void HistogramCorrection()
{
	int j = threadIdx.x;
	if (dev_M <= dev_MaxThreads)
	{
		if (threadIdx.x < dev_M)
		{
			for (int i = 0; i < dev_N; i++)
			{
				dev_histogram[i][j] = (dev_Max * (dev_grayScale[i][j] - dev_globalMin[0]) / (dev_globalMax[0] - dev_globalMin[0]));
			}
		}
	}
	else
	{
		int run = __float2int_ru(dev_M / dev_MaxThreads);
		for (int block_i = 0; block_i < run; block_i++)
		{
			int modifier = block_i * dev_MaxThreads;
			if (j + modifier < dev_M)
			{
				for (int i = 0; i < dev_N; i++)
				{
					dev_histogram[i][j + modifier] = (dev_Max * (dev_grayScale[i][j + modifier] - dev_globalMin[0]) / (dev_globalMax[0] - dev_globalMin[0]));
				}
			}
		}
	}
}

void HistogramCorrection_CPU()
{
	for (int i = 0; i < N; i++)
	{
		for (int j = 0; j < M; j++)
		{
			histogram[i][j] = (RND_Max * (grayScale[i][j] - globalMin[0]) / (globalMax[0] - globalMin[0]));
		}
	}
}

//********************************************************************************************************************/
//Szín invertálás (ha kell):

__global__ void DarkPixelNumber()
{
	int j = threadIdx.x;
	if (dev_M <= dev_MaxThreads)
	{
		if (j < dev_M)
		{
			for (int i = 0; i < dev_N; i++)
			{
				if (dev_histogram[i][j] <= 127)
				{
					atomicAdd(&dev_darkPixelCounter[0], 1);
				}
			}
		}
	}
	else
	{
		int run = __float2int_ru(dev_M / dev_MaxThreads);
		for (int block_i = 0; block_i < run; block_i++)
		{
			int modifier = block_i * dev_MaxThreads;

			if (j + modifier < dev_M)
			{
				for (int i = 0; i < dev_N; i++)
				{
					if (dev_histogram[i][j + modifier] <= 127)
					{
						atomicAdd(&dev_darkPixelCounter[0], 1);
					}
				}
			}
		}
	}
}

void DarkPixelNumber_CPU()
{
	for (int i = 0; i < N; i++)
	{
		for (int j = 0; j < M; j++)
		{
			if (histogram[i][j] <= 127)
			{
				darkPixelCounter[0]++;
			}
		}
	}
}

__global__ void ColorInverter()
{
	int j = threadIdx.x;
	if (dev_M <= dev_MaxThreads)
	{
		if (j < dev_M)
		{
			for (int i = 0; i < N; i++)
			{
				dev_histogram[i][j] = 255 - dev_histogram[i][j];
			}
		}
	}
	else
	{
		int run = __float2int_ru(dev_M / dev_MaxThreads);
		for (int block_i = 0; block_i < run; block_i++)
		{
			int modifier = block_i * dev_MaxThreads;
			if (j + modifier < dev_M)
			{
				for (int i = 0; i < N; i++)
				{
					dev_histogram[i][j + modifier] = 255 - dev_histogram[i][j + modifier];
				}
			}
		}
	}
}

void ColorInverter_CPU()
{
	for (int i = 0; i < N; i++)
	{
		for (int j = 0; j < M; j++)
		{
			histogram[i][j] = 255 - histogram[i][j];
		}
	}
}

//********************************************************************************************************************/
//Zajszûrés:

//Gauss osztó kiszámítása:

__global__ void GetGaussValue()
{
	int i = threadIdx.x / dev_GaussSize;
	int j = threadIdx.x - (i * dev_GaussSize);

	atomicAdd(&dev_GaussValue[0], dev_GaussMatrix[i][j]);
}

void GetGaussValue_CPU()
{
	for (int i = 0; i < GaussSize; i++)
	{
		for (int j = 0; j < GaussSize; j++)
		{
			GaussValue[0] += GaussMatrix[i][j];
		}
	}
}

//Gauss Mátrix - Zajszûrés:

__global__ void GaussTransformation()
{
	int j = threadIdx.x;
	int diff = dev_GaussSize / 2;

	if (dev_M <= dev_MaxThreads)
	{
		if (j < dev_M)
		{
			for (int i = 0; i < dev_N; i++)
			{
				int sum = 0;
				int gauss_i = 0;
				int gauss_j = 0;

				for (int img_i = i - diff; img_i < i + diff; img_i++)
				{
					gauss_j = 0;
					for (int img_j = j - diff; img_j < j + diff; img_j++)
					{
						int originalValue = 0;

						if (0 <= img_i && 0 <= img_j && img_i < dev_N && img_j < dev_M)
						{
							originalValue = dev_histogram[img_i][img_j];
						}

						sum += dev_GaussMatrix[gauss_i][gauss_j] * originalValue;
						gauss_j++;
					}
					gauss_i++;
				}

				dev_noNoise[i][j] = sum / dev_GaussValue[0];
			}
		}
	}
	else
	{
		int run = __float2int_ru(dev_M / dev_MaxThreads);
		for (int block_i = 0; block_i < run; block_i++)
		{
			int modifier = block_i * dev_MaxThreads;
			if (j + modifier < dev_M)
			{
				for (int i = 0; i < dev_N; i++)
				{
					int sum = 0;
					int gauss_i = 0;
					int gauss_j = 0;

					for (int img_i = i - diff; img_i < i + diff; img_i++)
					{
						gauss_j = 0;
						for (int img_j = j + modifier - diff; img_j < j + modifier + diff; img_j++)
						{
							int originalValue = 0;

							if (0 <= img_i && 0 <= img_j && img_i < dev_N && img_j < dev_M)
							{
								originalValue = dev_histogram[img_i][img_j];
							}

							sum += dev_GaussMatrix[gauss_i][gauss_j] * originalValue;
							gauss_j++;
						}
						gauss_i++;
					}

					dev_noNoise[i][j + modifier] = sum / dev_GaussValue[0];
				}
			}
		}
	}
}

void GaussTransformation_CPU()
{
	int diff = dev_GaussSize / 2;
	for (int i = 0; i < N; i++)
	{
		for (int j = 0; j < M; j++)
		{
			int sum = 0;
			int gauss_i = 0;
			int gauss_j = 0;

			for (int img_i = i - diff; img_i < i + diff; img_i++)
			{
				gauss_j = 0;
				for (int img_j = j - diff; img_j < j + diff; img_j++)
				{
					int originalValue = 0;

					if (0 <= img_i && 0 <= img_j && img_i < dev_N && img_j < dev_M)
					{
						originalValue = histogram[img_i][img_j];
					}

					sum += GaussMatrix[gauss_i][gauss_j] * originalValue;
					gauss_j++;
				}
				gauss_i++;
			}

			noNoise[i][j] = sum / GaussValue[0];
		}
	}
}

//********************************************************************************************************************/
//To Black And White:

//Átlagos pixel érték

__global__ void AVGPixelColor()
{
	int j = threadIdx.x;

	if (dev_M <= dev_MaxThreads)
	{
		if (j < dev_M)
		{
			for (int i = 0; i < dev_N; i++)
			{
				//256 * 0,8 = 204,8 
				if (dev_noNoise[i][j] <= 205)
				{
					atomicAdd(&dev_avgPixelColor[0], dev_noNoise[i][j]);
					atomicAdd(&dev_avgPixelColorCounter[0], 1);
					__syncthreads();
				}
			}
		}
	}
	else
	{
		int run = __float2int_ru(dev_M / dev_MaxThreads);
		for (int block_i = 0; block_i < run; block_i++)
		{
			int modifier = block_i * dev_MaxThreads;
			if (j + modifier < dev_M)
			{
				for (int i = 0; i < dev_N; i++)
				{
					//256 * 0,8 = 204,8 
					if (dev_noNoise[i][j + modifier] <= 205)
					{
						atomicAdd(&dev_avgPixelColor[0], dev_noNoise[i][j + modifier]);
						atomicAdd(&dev_avgPixelColorCounter[0], 1);
						__syncthreads();
					}
				}
			}
		}
	}
	__syncthreads();
	if (threadIdx.x == 0)
	{
		dev_avgPixelColor[0] = __float2int_rn(dev_avgPixelColor[0] / dev_avgPixelColorCounter[0]);
	}
}

void AVGPixelColor_CPU()
{
	for (int i = 0; i < N; i++)
	{
		for (int j = 0; j < M; j++)
		{
			if (dev_noNoise[i][j] <= 205)
			{
				avgPixelColor[0] += noNoise[i][j];
				avgPixelColorCounter[0]++;
			}
		}
	}

	avgPixelColor[0] = round(avgPixelColor[0] / avgPixelColorCounter[0]);
}

//Konvertálás 0-ra és 255-re

__global__ void ConvertToBlackAndWhite()
{
	int j = threadIdx.x;
	if (dev_M <= dev_MaxThreads)
	{
		if (j < dev_M)
		{
			for (int i = 0; i < dev_N; i++)
			{
				if (dev_noNoise[i][j] <= dev_avgPixelColor[0])
				{
					dev_blackAndWhite[i][j] = 0;
				}
				else
				{
					dev_blackAndWhite[i][j] = 255;
				}
			}
		}
	}
	else
	{
		int run = __float2int_ru(dev_M / dev_MaxThreads);
		for (int block_i = 0; block_i < run; block_i++)
		{
			int modifier = block_i * dev_MaxThreads;
			if (j + modifier < dev_M)
			{
				for (int i = 0; i < dev_N; i++)
				{
					if (dev_noNoise[i][j + modifier] <= dev_avgPixelColor[0])
					{
						dev_blackAndWhite[i][j + modifier] = 0;
					}
					else
					{
						dev_blackAndWhite[i][j + modifier] = 255;
					}
				}
			}
		}
	}
}

void ConvertToBlackAndWhite_CPU()
{
	for (int i = 0; i < N; i++)
	{
		for (int j = 0; j < M; j++)
		{
			if (noNoise[i][j] <= avgPixelColor[0])
			{
				blackAndWhite[i][j] = 0;
			}
			else
			{
				blackAndWhite[i][j] = 255;
			}
		}
	}
}

//********************************************************************************************************************/
//Konvertálás értékmátrixá:

__global__ void ConvertToValueMatrix()
{
	int j = threadIdx.x;
	if (dev_M <= dev_MaxThreads)
	{
		if (j < dev_M)
		{
			for (int i = 0; i < dev_N; i++)
			{
				if (dev_blackAndWhite[i][j] == 0)
				{
					dev_valueMatrix[i][j] = -3;
				}
				else
				{
					dev_valueMatrix[i][j] = 0;
				}
			}
		}
	}
	else
	{
		int run = __float2int_ru(dev_M / dev_MaxThreads);
		for (int block_i = 0; block_i < run; block_i++)
		{
			int modifier = block_i * dev_MaxThreads;
			if (j + modifier < dev_M)
			{
				for (int i = 0; i < dev_N; i++)
				{
					if (dev_blackAndWhite[i][j + modifier] == 0)
					{
						dev_valueMatrix[i][j + modifier] = -3;
					}
					else
					{
						dev_valueMatrix[i][j + modifier] = 0;
					}
				}
			}
		}
	}
}

void ConvertToValueMatrix_CPU()
{
	for (int i = 0; i < N; i++)
	{
		for (int j = 0; j < M; j++)
		{
			if (blackAndWhite[i][j] == 0)
			{
				valueMatrix[i][j] = -3;
			}
			else
			{
				valueMatrix[i][j] = 0;
			}
		}
	}
}

//********************************************************************************************************************/
//Text file létrehozása, generálása:


//********************************************************************************************************************//
//Kiiratás:

void Console_WriteLine(float arr[N][M])
{
	for (int i = 0; i < N; i++)
	{
		for (int j = 0; j < M; j++)
		{
			printf("%003.0f	", arr[i][j]);
		}
		printf("\n");
	}
	printf("\n");
}

void Console_WriteLine(int arr[N][M])
{
	for (int i = 0; i < N; i++)
	{
		for (int j = 0; j < M; j++)
		{
			printf("%003d	", arr[i][j]);
		}
		printf("\n");
	}
	printf("\n");
}

void Console_WriteLine(int vector[NM])
{
	for (int i = 0; i < NM; i++)
	{
		printf("%003d	", vector[i]);
	}
	printf("\n\n");
}

void Console_WriteLine(int number)
{
	printf("%003d", number);
	printf("\n\n");
}

void Console_WriteLine(char text[])
{
	printf("%s\n", text);
}

void Console_WriteLine(char text[], int number)
{
	printf("%s %d\n", text, number);
}

void Console_WriteLine(char text[], float number)
{
	printf("%s %f\n", text, number);
}

void Console_WriteLine(bool isCPU, char functionName[])
{
	if (isCPU)
	{
		printf("[CPU] - %s\n", functionName);
	}
	else
	{
		printf("[GPU] - %s\n", functionName);
	}
}

void Console_WriteLine(bool isCPU, char functionName[], float time)
{
	if (isCPU)
	{
		printf("[CPU] - %s, time: %f millisec\n", functionName, time);
	}
	else
	{
		printf("[GPU] - %s, time: %f millisec\n", functionName, time);
	}
}


void ClockStart()
{
	cudaEventRecord(start);
}

void ClockStop(float& milliseconds)
{
	milliseconds = 0;
	cudaEventRecord(stop);
	cudaEventSynchronize(stop);
	cudaEventElapsedTime(&milliseconds, start, stop);
}

//********************************************************************************************************************//
//Main:

int main()
{

	float v_threshold = 10;
	//load the image
	//Mat img = imread("lena.jpg");
	Mat img = imread("C:\\Users\\loahc\\Documents\\GitHub\\Thesis_HJC885\\Unity\\Thesis_HJC885\\Assets\\screenshots\\testpic\\screen_1024x768_2019-11-25_15-22-04_01.png");
	Mat img2 = imread("C:\\Users\\loahc\\Documents\\GitHub\\Thesis_HJC885\\Unity\\Thesis_HJC885\\Assets\\screenshots\\testpic\\screen_1024x768_2019-11-25_15-22-04_02.png");
	
	namedWindow("pic_1", WINDOW_NORMAL);
	resizeWindow("pic_1", img.cols, img.rows);
	imshow("pic_1", img);

	namedWindow("newpic", WINDOW_NORMAL);
	resizeWindow("newpic", img2.cols, img2.rows);
	imshow("newpic", img2);

	waitKey(0);

	//read height, width data
//	Kernel();

	int height = img.rows;
	int width = img.cols;




	//check whether image loaded is empty or not.
	if (img.empty())
	{
		cerr << "no image"; return -1;
	}

	//convert image from CV::MAT to float*.
	Mat dst;
	img.convertTo(dst, CV_32F);
	float* data = dst.ptr<float>();

	Mat dst2;
	img2.convertTo(dst2, CV_32F);
	float* data2 = dst2.ptr<float>();


		//sub

	for (int i = 0; i < (height * width) * 3; i += 3)
	{
		float r = abs((data[i] - data2[i]) / 2);
		float g = abs((data[i + 1] - data2[i + 1]) / 2);
		float b = abs((data[i + 2] - data2[i + 2]) / 2);

		//float avg = (r + g + b) / 3;
		data[i] = r;
		data[i + 1] = g;
		data[i + 2] = b;
	}


	//convert back the image from float* to CV::MAT.
	Mat dest(height, width, CV_32FC3, data);

	//print the image
	imwrite("subpic.jpg", dest);
	Mat img3 = imread("subpic.jpg");
	namedWindow("subpic", WINDOW_NORMAL);
	resizeWindow("subpic", img3.cols, img3.rows);
	imshow("subpic", img3);

		//Makewhite and black only
	for (int i = 0; i < (height * width) * 3; i += 3)
	{

		float r = data[i];
		float g = data[i + 1];
		float b = data[i + 2];
		if (r >= v_threshold || g >= v_threshold || b >= v_threshold)
		{
			data[i] = 255;
			data[i + 1] = 255;
			data[i + 2] = 255;
		}


	}
	Mat dest2(height, width, CV_32FC3, data);

	//print the image
	imwrite("blackandwhite.jpg", dest2);
	Mat img5 = imread("blackandwhite.png",1);
	Mat img4 = imread("blackandwhite.jpg");
	printf("%i", img5.type());

	
	
	namedWindow("blackandwhite", WINDOW_NORMAL);
	resizeWindow("blackandwhite", img4.cols, img4.rows);
	imshow("blackandwhite", img5);
	waitKey(0);
	
	Mat src_gray;
	src_gray.convertTo(img5, CV_8U);

		cvtColor(src_gray, img5, COLOR_RGB2GRAY);
	


	/// Convert it to gray
	//cvtColor(img5, src_gray, COLOR_RGB2GRAY);

	/// Reduce the noise so we avoid false circle detection
	GaussianBlur(src_gray, src_gray, Size(9, 9), 2, 2);

	vector<Vec3f> circles;

	HoughCircles(img5, circles, HOUGH_GRADIENT, 1, src_gray.rows / 8, 200, 100, 0, 0);



	if (circles.size() == 0)
	{
		printf("No circles");
			return(-1);
	}
	int radius;
	for (int i = 0; i < circles.size(); i++)
	{
		Point center1(cvRound(circles[i][0]), cvRound(circles[i][1]));
		radius = cvRound(circles[i][2]);
		circle(img5, center1, 3, Scalar(0, 255, 0), -1, 8, 0);
		circle(img5, center1, radius, Scalar(255, 0, 0), 3, 8, 0);
		if (i>0)
		{
			Point center1(cvRound(circles[i-1][0]), cvRound(circles[i-1][1]));
			Point center2(cvRound(circles[i][0]), cvRound(circles[i][1]));
	
			line(img5, center1, center2, Scalar(255, 0, 255), 3, 8, 0);
		}
	}

	namedWindow("CIRCLES", WINDOW_NORMAL);
	resizeWindow("CIRCLES", img5.cols, img5.rows);
	imshow("CIRCLES", img5);
	waitKey(0);

	//////////////////////////////////////////////INNEN START////////////////////////////////////////////////////////////
	cudaEventCreate(&start);
	cudaEventCreate(&stop);

	float milliseconds = 0;

	Console_WriteLine("Picture Size N: ", N);
	Console_WriteLine("Picture Size M: ", M);

	Console_WriteLine("");
	Console_WriteLine("");

	MEASURE_TIME(1, "RandomPicture_CPU", RandomPicture_CPU());


	ClockStart();
	cudaMemcpyToSymbol(dev_channel_r, channel_r, N * M * sizeof(float));
	cudaMemcpyToSymbol(dev_channel_g, channel_g, N * M * sizeof(float));
	cudaMemcpyToSymbol(dev_channel_b, channel_b, N * M * sizeof(float));
	ClockStop(milliseconds);
	Console_WriteLine(false, "Copy", milliseconds);

	ClockStart();
	GrayScale << < 1, MaxThreads >> > ();
	ClockStop(milliseconds);
	Console_WriteLine(false, "Grayscale", milliseconds);



	ClockStart();
	ConvertArrayToVector << < 1, MaxThreads >> > ();

	ClockStop(milliseconds);
	Console_WriteLine(false, "Convert Array To Vector", milliseconds);



	ClockStart();
	MinSearch << < 1, MaxThreads >> > ();

	ClockStop(milliseconds);
	Console_WriteLine(false, "Minimum Search", milliseconds);



	ClockStart();
	MaxSearch << < 1, MaxThreads >> > ();

	ClockStop(milliseconds);
	Console_WriteLine(false, "Maximum Search", milliseconds);



	ClockStart();
	HistogramCorrection << < 1, MaxThreads >> > ();

	ClockStop(milliseconds);
	Console_WriteLine(false, "Historam Correction", milliseconds);



	ClockStart();
	DarkPixelNumber << <1, MaxThreads >> > ();

	ClockStop(milliseconds);
	Console_WriteLine(false, "Dark Pixel Counter", milliseconds);

	cudaMemcpyFromSymbol(res_darkPixelCounter, dev_darkPixelCounter, 1 * sizeof(int));

	if (NM / 2 < res_darkPixelCounter[0])
	{
		ClockStart();
		ColorInverter << <1, MaxThreads >> > ();

		ClockStop(milliseconds);
		Console_WriteLine(false, "Color Inverter", milliseconds);
	}



	ClockStart();
	GetGaussValue << <1, GaussSize* GaussSize >> > ();

	ClockStop(milliseconds);
	Console_WriteLine(false, "Gauss Value", milliseconds);



	ClockStart();
	GaussTransformation << <1, MaxThreads >> > ();

	ClockStop(milliseconds);
	Console_WriteLine(false, "Gauss Transformation", milliseconds);



	ClockStart();
	AVGPixelColor << <1, MaxThreads >> > ();

	ClockStop(milliseconds);
	Console_WriteLine(false, "AVG Pixel Color", milliseconds);



	ClockStart();
	ConvertToBlackAndWhite << <1, MaxThreads >> > ();

	ClockStop(milliseconds);
	Console_WriteLine(false, "Convert To Black And White", milliseconds);



	ClockStart();
	ConvertToValueMatrix << <1, MaxThreads >> > ();

	ClockStop(milliseconds);
	Console_WriteLine(false, "Convert To Value Matrix", milliseconds);



	Console_WriteLine("");
	Console_WriteLine("All GPU Process Finished");
	Console_WriteLine("");



	ClockStart();
	cudaMemcpyFromSymbol(res_grayScale, dev_grayScale, N * M * sizeof(int));
	cudaMemcpyFromSymbol(res_forMinMaxSearch, dev_forMinMaxSearch, N * M * sizeof(int));
	cudaMemcpyFromSymbol(res_globalMin, dev_globalMin, 1 * sizeof(int));
	cudaMemcpyFromSymbol(res_globalMax, dev_globalMax, 1 * sizeof(int));
	cudaMemcpyFromSymbol(res_histogram, dev_histogram, N * M * sizeof(int));
	cudaMemcpyFromSymbol(res_GaussValue, dev_GaussValue, 1 * sizeof(int));
	cudaMemcpyFromSymbol(res_noNoise, dev_noNoise, N * M * sizeof(int));
	cudaMemcpyFromSymbol(res_avgPixelColor, dev_avgPixelColor, 1 * sizeof(int));
	cudaMemcpyFromSymbol(res_blackAndWhite, dev_blackAndWhite, N * M * sizeof(int));
	cudaMemcpyFromSymbol(res_valueMatrix, dev_valueMatrix, N * M * sizeof(int));

	ClockStop(milliseconds);
	Console_WriteLine(false, "All Value Copy Back", milliseconds);


	MEASURE_TIME(1, "GrayScale_CPU", GrayScale_CPU());
	MEASURE_TIME(1, "MinSearch_CPU", MinSearch_CPU());
	MEASURE_TIME(1, "MaxSearch_CPU", MaxSearch_CPU());
	MEASURE_TIME(1, "HistogramCorrection_CPU", HistogramCorrection_CPU());
	MEASURE_TIME(1, "DarkPixelNumber_CPU", DarkPixelNumber_CPU());

	if (NM / 2 < darkPixelCounter[0])
	{
		MEASURE_TIME(1, "ColorInverter_CPU", ColorInverter_CPU());
	}

	MEASURE_TIME(1, "GetGaussValue_CPU", GetGaussValue_CPU());
	MEASURE_TIME(1, "GaussTransformation_CPU", GaussTransformation_CPU());
	MEASURE_TIME(1, "AVGPixelColor_CPU", AVGPixelColor_CPU());
	MEASURE_TIME(1, "ConvertToBlackAndWhite_CPU", ConvertToBlackAndWhite_CPU());
	MEASURE_TIME(1, "ConvertToValueMatrix_CPU", ConvertToValueMatrix_CPU());

	/*
	Console_WriteLine(channel_r);

	Console_WriteLine(channel_g);

	Console_WriteLine(channel_b);

	Console_WriteLine(grayScale);

	Console_WriteLine(forMinMaxSearch);

	Console_WriteLine(globalMin[0]);

	Console_WriteLine(globalMax[0]);

	Console_WriteLine(histogram);

	Console_WriteLine(GaussValue[0]);

	Console_WriteLine(noNoise);

	Console_WriteLine(avgPixelColor[0]);

	Console_WriteLine(blackAndWhite);

	Console_WriteLine(valueMatrix);
	*/


	return 0;
}