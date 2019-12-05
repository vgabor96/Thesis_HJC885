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


const int RND_Min = 0;
const int RND_Max = 255;
const int MaxThreads = 1024;

__device__ const int dev_N = 768;
__device__ const int dev_M = 1024;
__device__ const int dev_NM = dev_N * dev_M;

__device__ const int dev_MaxThreads = 1024;
__device__ const int dev_Max = 255;


//Contributed by Pakos

cudaEvent_t start;
cudaEvent_t stop;


float channel_r[N][M];
float channel_g[N][M];
float channel_b[N][M];


int grayScale[N][M];    

int forMinMaxSearch[NM];

int histogram[N][M];    

int noNoise[N][M];		

int blackAndWhite[N][M]; 

int valueMatrix[N][M];	 


int globalMin[1];		  
int globalMax[1];	      

int darkPixelCounter[1] = { 0 };

int avgPixelColor[1] = { 0 };
int avgPixelColorCounter[1] = { 0 };


int GaussSize = 3;
int GaussValue[1] = { 0 };
int GaussMatrix[3][3] =
{
	{1, 2, 1},
	{2, 4, 2},
	{1, 2, 1}
};





__device__ float dev_channel_r[N][M];
__device__ float dev_channel_g[N][M];
__device__ float dev_channel_b[N][M];

__device__ int dev_grayScale[N][M];    

__device__ int dev_forMinMaxSearch[NM]; 

__device__ int dev_histogram[N][M];    

__device__ int dev_noNoise[N][M];

__device__ int dev_blackAndWhite[N][M];

__device__ int dev_valueMatrix[N][M];


int res_grayScale[N][M];

int res_forMinMaxSearch[NM];

int res_histogram[N][M];

int res_noNoise[N][M];

int res_blackAndWhite[N][M];

int res_valueMatrix[N][M];


__device__ int localMin[(NM + 1) / 2];
__device__ int localMax[(NM + 1) / 2];

__device__ int dev_globalMin[1];		  
__device__ int dev_globalMax[1];	     

__device__ int dev_darkPixelCounter[1] = { 0 };

__device__ int dev_avgPixelColor[1] = { 0 };
__device__ int dev_avgPixelColorCounter[1] = { 0 };


int res_globalMin[1];		
int res_globalMax[1];

int res_darkPixelCounter[1] = { 0 };

int res_avgPixelColor[1] = { 0 };


__device__ int dev_GaussSize = 3;
__device__ int dev_GaussValue[1] = { 0 };
__device__ int dev_GaussMatrix[3][3] =
{
	{1, 2, 1},
	{2, 4, 2},
	{1, 2, 1}
};

int res_GaussValue[1] = { 0 };


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


void LoadPicture_CPU(Mat img)
{
	Mat img2 = imread("C:\\Users\\loahc\\Documents\\GitHub\\Thesis_HJC885\\Unity\\Thesis_HJC885\\Assets\\screenshots\\testpic\\screen_1024x768_2019-11-25_15-22-04_01.png");
	Mat dst;
	img2.convertTo(dst, CV_32F);
	float* data = dst.ptr<float>();

	int x = 0;

	for (int i = 0; i < N; i++)
	{
		for (int j = 0; j < M; j++)
		{

			//channel_r[i][j] = data[i * M + j *3];
			channel_r[i][j] = data[x];
			channel_g[i][j] = data[x + 1];
			channel_b[i][j] = data[x + 2];
			x += 3;

		}
	
	
	}
								
}

Mat LoadBackPicture()
{
	Mat img,r,g,b;

	r = Mat(N, M, CV_32FC1, channel_r,0);
	g = Mat(N, M, CV_32FC1, channel_g,0);
	b = Mat(N, M, CV_32FC1, channel_b,0);

	
	vector<Mat> channels;
	

	channels.push_back(r);
	channels.push_back(g);
	channels.push_back(b);

	merge(channels, img);


	return img;

}

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



__global__ void AVGPixelColor()
{
	int j = threadIdx.x;

	if (dev_M <= dev_MaxThreads)
	{
		if (j < dev_M)
		{
			for (int i = 0; i < dev_N; i++)
			{
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

void Custom_printf(float arr[N][M])
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

void Custom_printf(int arr[N][M])
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

void Custom_printf(int vector[NM])
{
	for (int i = 0; i < NM; i++)
	{
		printf("%003d	", vector[i]);
	}
	printf("\n\n");
}

void Custom_printf(int number)
{
	printf("%003d", number);
	printf("\n\n");
}

void Custom_printf(char text[])
{
	printf("%s\n", text);
}

void Custom_printf(char text[], int number)
{
	printf("%s %d\n", text, number);
}

void Custom_printf(char text[], float number)
{
	printf("%s %f\n", text, number);
}

void Custom_printf(bool isCPU, char functionName[])
{
	if (isCPU)
	{
		printf("[CPU] - %s\n", functionName);
	}
	else
	{
		printf("|GPU| - %s\n", functionName);
	}
}

void Custom_printf(bool isCPU, char functionName[], float time)
{
	if (isCPU)
	{
		printf("[CPU] - %s, time: %f millisec\n", functionName, time);
	}
	else
	{
		printf("|GPU| - %s, time: %f millisec\n", functionName, time);
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


int main()
{

	float v_threshold = 10;

	Mat img = imread("C:\\Users\\loahc\\Documents\\GitHub\\Thesis_HJC885\\Unity\\Thesis_HJC885\\Assets\\screenshots\\testpic\\screen_1024x768_2019-11-25_15-22-04_01.png");
	Mat img2 = imread("C:\\Users\\loahc\\Documents\\GitHub\\Thesis_HJC885\\Unity\\Thesis_HJC885\\Assets\\screenshots\\testpic\\screen_1024x768_2019-11-25_15-22-04_02.png");
	
	Custom_printf("Picture Size N: ", N);
	Custom_printf("Picture Size M: ", M);

	namedWindow("pic_1", WINDOW_NORMAL);
	resizeWindow("pic_1", img.cols, img.rows);
	imshow("pic_1", img);

	namedWindow("newpic", WINDOW_NORMAL);
	resizeWindow("newpic", img2.cols, img2.rows);
	imshow("newpic", img2);




	int height = img.rows;
	int width = img.cols;

	Mat imgadd = imread("C:\\Users\\loahc\\Documents\\GitHub\\Thesis_HJC885\\Unity\\Thesis_HJC885\\Assets\\screenshots\\testpic\\screen_1024x768_2019-11-25_15-22-04_01.png");
	Mat img2add = imread("C:\\Users\\loahc\\Documents\\GitHub\\Thesis_HJC885\\Unity\\Thesis_HJC885\\Assets\\screenshots\\testpic\\screen_1024x768_2019-11-25_15-22-04_02.png");

	//convert image from CV::MAT to float*.
	Mat dstadd;
	imgadd.convertTo(dstadd, CV_32F);
	float* dataadd = dstadd.ptr<float>();

	Mat dst2add;
	img2add.convertTo(dst2add, CV_32F);
	float* data2add = dst2add.ptr<float>();


	//ADDED

	for (int i = 0; i < (height * width) * 3; i += 3)
	{
		float r = (dataadd[i] + data2add[i]) / 2;
		float g = (dataadd[i + 1] + data2add[i + 1]) / 2;
		float b = (dataadd[i + 2] + data2add[i + 2]) / 2;


		dataadd[i] = r;
		dataadd[i + 1] = g;
		dataadd[i + 2] = b;
	}


	//convert back the image from float* to CV::MAT.
	Mat destadd(height, width, CV_32FC3, dataadd);

	//print the image
	imwrite("addpic.jpg", destadd);
	Mat img3add = imread("addpic.jpg");
	namedWindow("AddedPic", WINDOW_NORMAL);
	resizeWindow("AddedPic", img3add.cols, img3add.rows);
	imshow("AddedPic", img3add);


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
	//Mat img5 = imread("blackandwhite.png");
	Mat img4 = imread("blackandwhite.jpg");

	Mat src, src_gray;

	/// Read the image
	src = imread("blackandwhite.jpg", 1);

	if (!src.data)
	{
		return -1;
	}

	/// Convert it to gray
	cvtColor(src, src_gray, COLOR_RGB2GRAY);
	namedWindow("Hough Circle Transform Gray", WINDOW_NORMAL);
	imshow("Hough Circle Transform Gray", src);

	/// Reduce the noise so we avoid false circle detection
	GaussianBlur(src_gray, src_gray, Size(9, 9), 2, 2);
	namedWindow("Hough Circle Transform Gauss", WINDOW_NORMAL);
	imshow("Hough Circle Transform Gauss", src);

	vector<Vec3f> circles;
	for (int maxR = 10; maxR < 200; maxR = maxR + 200-9)
	{

		/// Apply the Hough Transform to find the circles
		//		WorkingHoughCircles(src_gray, circles, HOUGH_GRADIENT, 1, src_gray.rows / 100, 10, 10, 0, 100);
		HoughCircles(src_gray, circles, HOUGH_GRADIENT, 1, src_gray.rows / 100, 10, 10, 0, 0);

		/// Draw the circles detected
		for (size_t i = 0; i < circles.size(); i++)
		{
			Point center(cvRound(circles[i][0]), cvRound(circles[i][1]));
			int radius = cvRound(circles[i][2]);
			// circle center
			circle(src, center, 2, Scalar(0, 255, 0),2, 8, 0);
			// circle outline
			circle(src, center, radius, Scalar(0, 0, 255), 2, 8, 0);
			if (i > 0)
			{
				Point center1(cvRound(circles[i - 1][0]), cvRound(circles[i - 1][1]));
				printf("Point1 <%i, \t", center1.x);
				printf("%i >\n", center1.y);

				Point center2(cvRound(circles[i][0]), cvRound(circles[i][1]));

				printf("Point2 <%i, \t", center2.x);
				printf("%i >\n", center2.y);

				line(src, center1, center2, Scalar(0, 0, 255), 2, 8, 0);
			}
		}
	}

	/// Show your results
	namedWindow("Hough Circle Transform", WINDOW_NORMAL);
	imshow("Hough Circle Transform", src);



	Mat merged = imread("C:\\Users\\loahc\\Documents\\GitHub\\Thesis_HJC885\\Unity\\Thesis_HJC885\\Assets\\screenshots\\testpic\\screen_1024x768_2019-11-25_15-22-04_01.png");
	Mat imgsecond = imread("C:\\Users\\loahc\\Documents\\GitHub\\Thesis_HJC885\\Unity\\Thesis_HJC885\\Assets\\screenshots\\testpic\\screen_1024x768_2019-11-25_15-22-04_02.png");

	Mat dstmerged;
	merged.convertTo(dstmerged, CV_32F);
	float* datamerged = dstmerged.ptr<float>();

	Mat dstsecond;
	imgsecond.convertTo(dstsecond, CV_32F);
	float* data2second = dstsecond.ptr<float>();

	Mat dstcircles;
	src.convertTo(dstcircles, CV_32F);
	float* datamcircles = dstcircles.ptr<float>();

	//add

	for (int i = 0; i < (height * width) * 3; i += 3)
	{
		float r = (datamerged[i] + data2second[i] + datamcircles[i]) / 3;
		float g = (datamerged[i+1] + data2second[i + 1] + datamcircles[i + 1] )/ 3;
		float b = (datamerged[i + 2] + data2second[i + 2] + datamcircles[i + 2]) / 3;

		//float avg = (r + g + b) / 3;
		datamerged[i] = r;
		datamerged[i + 1] = g;
		datamerged[i + 2] = b;
	}




	//////////////////////////////////////////////INNEN START////////////////////////////////////////////////////////////
	cudaEventCreate(&start);
	cudaEventCreate(&stop);


	float milliseconds = 0;



	Custom_printf("");
	Custom_printf("");

	MEASURE_TIME(1, "LoadPicture_CPU", RandomPicture_CPU());

	//MEASURE_TIME(1, "LoadedPicture_CPU", LoadPicture_CPU(img));





	ClockStart();
	cudaMemcpyToSymbol(dev_channel_r, channel_r, N * M * sizeof(float));
	cudaMemcpyToSymbol(dev_channel_g, channel_g, N * M * sizeof(float));
	cudaMemcpyToSymbol(dev_channel_b, channel_b, N * M * sizeof(float));
	ClockStop(milliseconds);
	Custom_printf(false, "Copy", milliseconds);

	ClockStart();
	GrayScale << < 1, MaxThreads >> > ();
	ClockStop(milliseconds);
	Custom_printf(false, "Grayscale", milliseconds);




	ConvertArrayToVector << < 1, MaxThreads >> > ();


	ClockStart();
	MinSearch << < 1, MaxThreads >> > ();

	ClockStop(milliseconds);
	Custom_printf(false, "Minimum Search", milliseconds);



	ClockStart();
	MaxSearch << < 1, MaxThreads >> > ();

	ClockStop(milliseconds);
	Custom_printf(false, "Maximum Search", milliseconds);


	ClockStart();
	DarkPixelNumber << <1, MaxThreads >> > ();

	ClockStop(milliseconds);
	Custom_printf(false, "Dark Pixel Counter", milliseconds);

	cudaMemcpyFromSymbol(res_darkPixelCounter, dev_darkPixelCounter, 1 * sizeof(int));

	if (NM / 2 < res_darkPixelCounter[0])
	{
		ClockStart();
		ColorInverter << <1, MaxThreads >> > ();

		ClockStop(milliseconds);
		Custom_printf(false, "Color Inverter", milliseconds);
	}



	ClockStart();
	GetGaussValue << <1, GaussSize* GaussSize >> > ();

	ClockStop(milliseconds);
	Custom_printf(false, "Gauss Value", milliseconds);



	ClockStart();
	GaussTransformation << <1, MaxThreads >> > ();

	ClockStop(milliseconds);
	Custom_printf(false, "Gauss Transformation", milliseconds);



	ClockStart();
	AVGPixelColor << <1, MaxThreads >> > ();

	ClockStop(milliseconds);
	Custom_printf(false, "AVG Pixel Color", milliseconds);



	ClockStart();
	ConvertToBlackAndWhite << <1, MaxThreads >> > ();

	ClockStop(milliseconds);
	Custom_printf(false, "Convert To Black And White", milliseconds);



	ClockStart();
	ConvertToValueMatrix << <1, MaxThreads >> > ();

	ClockStop(milliseconds);
	Custom_printf(false, "Convert To Value Matrix", milliseconds);



	Custom_printf("");
	Custom_printf("All GPU Process Finished");
	Custom_printf("");



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
	Custom_printf(false, "All Value Copy Back", milliseconds);


	MEASURE_TIME(1, "GrayScale_CPU", GrayScale_CPU());
	MEASURE_TIME(1, "MinSearch_CPU", MinSearch_CPU());
	MEASURE_TIME(1, "MaxSearch_CPU", MaxSearch_CPU());
	MEASURE_TIME(1, "DarkPixelNumber_CPU", DarkPixelNumber_CPU());


	MEASURE_TIME(1, "GetGaussValue_CPU", GetGaussValue_CPU());
	MEASURE_TIME(1, "GaussTransformation_CPU", GaussTransformation_CPU());
	MEASURE_TIME(1, "AVGPixelColor_CPU", AVGPixelColor_CPU());
	MEASURE_TIME(1, "ConvertToBlackAndWhite_CPU", ConvertToBlackAndWhite_CPU());

	//Custom_printf(channel_r);
	//Custom_printf(channel_g);
	//Custom_printf(channel_b);

	
	//Custom_printf(channel_r);

	//Custom_printf(channel_g);

	//Custom_printf(channel_b);

	//Custom_printf(grayScale);

	//Custom_printf(forMinMaxSearch);

	//Custom_printf(globalMin[0]);

	//Custom_printf(globalMax[0]);

	//Custom_printf(histogram);

	//Custom_printf(GaussValue[0]);

	//Custom_printf(noNoise);

	//Custom_printf(avgPixelColor[0]);

	//Custom_printf(blackAndWhite);

	//Custom_printf(valueMatrix);

	
	waitKey(0);
	return 0;
}