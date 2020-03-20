#include "cuda_runtime.h"
#include "device_launch_parameters.h"
#include <math.h>  
#include <io.h>
#include <stdio.h>
#include <stdlib.h> 
#include <time.h> 
#include "Timing.h"
#include <opencv2\core\mat.hpp>
#include <opencv2\core\cuda.inl.hpp>
#include <iostream>
#include <opencv2\highgui.hpp>
#include <opencv2\imgproc.hpp>
#include <string>
#include <vector>
using namespace std;
using namespace cv;


const int N = 768;
const int M = 1024;

int main()
{

	float v_threshold = 10;

	String folderpath = "C:\\Users\\loahc\\Documents\\GitHub\\Thesis_HJC885\\Unity\\Thesis_HJC885\\Assets\\screenshots\\temp\\*.png";
	vector<String> filenames;
	cv::glob(folderpath, filenames);

	for (size_t i = 1; i < filenames.size(); i++)
	{
		Mat img = imread(filenames[i - 1]);
		Mat img2 = imread(filenames[i]);

		printf("Picture Size N: %d \n", N);
		printf("Picture Size M: %d \n", M);

		namedWindow("pic_1", WINDOW_NORMAL);
		resizeWindow("pic_1", img.cols, img.rows);
		imshow("pic_1", img);

		namedWindow("pic_2", WINDOW_NORMAL);
		resizeWindow("pic_2", img2.cols, img2.rows);
		imshow("pic_2", img2);




		int height = img.rows;
		int width = img.cols;
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

		//convert back the image from float* to CV::MAT.
		Mat dest3(height, width, CV_32FC3, data);

		//print the image
		imwrite("pic1_black.jpg", dest3);
		Mat img4 = imread("pic1_black.jpg");
		namedWindow("pic1_black", WINDOW_NORMAL);
		resizeWindow("pic1_black", img4.cols, img4.rows);
		imshow("pic1_black", img4);

		Mat src, src_gray;

		/// Read the image
		src = img4;

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
		for (int maxR = 10; maxR < 200; maxR = maxR + 200 - 9)
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
				circle(src, center, 2, Scalar(0, 255, 0), 2, 8, 0);
				// circle outline
				circle(src, center, radius, Scalar(0, 0, 255), 2, 8, 0);
				if (i > 0)
				{
					Point center1(cvRound(circles[i - 1][0]), cvRound(circles[i - 1][1]));
					printf("%i,", center1.x);
					printf("%i,", center1.y);

					Point center2(cvRound(circles[i][0]), cvRound(circles[i][1]));

					printf("%i,", center2.x);
					printf("%i,true\n", center2.y);

					line(src, center1, center2, Scalar(0, 0, 255), 2, 8, 0);
				}
			}
		}

		/// Show your results
		namedWindow("Hough Circle Transform", WINDOW_NORMAL);
		imshow("Hough Circle Transform", src);

		waitKey(0);
	}


	/*Mat img = imread("C:\\Users\\loahc\\Documents\\GitHub\\Thesis_HJC885\\Unity\\Thesis_HJC885\\Assets\\screenshots\\testpic\\screen_1024x768_2019-11-25_15-22-04_01.png");
	Mat img2 = imread("C:\\Users\\loahc\\Documents\\GitHub\\Thesis_HJC885\\Unity\\Thesis_HJC885\\Assets\\screenshots\\testpic\\screen_1024x768_2019-11-25_15-22-04_02.png");
	*/

	return 0;
}