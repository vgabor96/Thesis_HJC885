#include<opencv2/opencv.hpp>
#include<iostream>
#include<cuda.h>
#include<cuda_runtime.h>
using namespace std;
using namespace cv;

__global__ void Kernel() {

	
};

//int main()
//{
//	float v_threshold = 10;
//	//load the image
//	//Mat img = imread("lena.jpg");
//	Mat img = imread("C:\\Users\\loahc\\Documents\\GitHub\\Thesis_HJC885\\Unity\\Thesis_HJC885\\Assets\\screenshots\\testpic\\screen_1024x768_2019-11-25_15-22-04_01.png");
//
//	namedWindow("pic_1", WINDOW_NORMAL);
//	resizeWindow("pic_1", img.cols, img.rows);
//	imshow("pic_1", img);
//
//	Mat img2 = imread("C:\\Users\\loahc\\Documents\\GitHub\\Thesis_HJC885\\Unity\\Thesis_HJC885\\Assets\\screenshots\\testpic\\screen_1024x768_2019-11-25_15-22-04_02.png");
//	namedWindow("newpic", WINDOW_NORMAL);
//	resizeWindow("newpic", img2.cols, img2.rows);
//	imshow("newpic", img2);
//
//	//read height, width data
////	Kernel();
//
//	int height = img.rows;
//	int width = img.cols;
//
//
//	//check whether image loaded is empty or not.
//	if (img.empty())
//	{
//		cerr << "no image"; return -1;
//	}
//
//	//convert image from CV::MAT to float*.
//	Mat dst;
//	img.convertTo(dst, CV_32F);
//	float* data = dst.ptr<float>();
//
//	Mat dst2;
//	img2.convertTo(dst2, CV_32F);
//	float* data2 = dst2.ptr<float>();
//
//	//Grayscale
//	/*for (int i = 0; i < (height*width)*3; i+=3)
//	{
//		float r = data[i];
//		float g = data[i + 1];
//		float b = data[i + 2];
//
//		float avg = (r + g + b) / 3;
//		data[i] = avg;
//		data[i + 1] = avg;
//		data[i + 2] = avg;
//	}
//	*/
//
//
//	//sub
//
//	for (int i = 0; i < (height * width) * 3; i += 3)
//	{
//		float r = abs((data[i] - data2[i]) / 2);
//		float g = abs((data[i + 1] - data2[i + 1]) / 2);
//		float b = abs((data[i + 2] - data2[i + 2]) / 2);
//
//		//float avg = (r + g + b) / 3;
//		data[i] = r;
//		data[i + 1] = g;
//		data[i + 2] = b;
//	}
//
//
//	//Add
//	/*
//	for (int i = 0; i < (height * width) * 3; i += 3)
//	{
//		float r = (data[i]+data2[i])/2;
//		float g = (data[i+1] + data2[i+1]) / 2;
//		float b = (data[i+2] + data2[i+2]) / 2;
//
//		float avg = (r + g + b) / 3;
//		data[i] = r;
//		data[i + 1] = g;
//		data[i + 2] = b;
//	}
//	*/
//
//
//
//	//convert back the image from float* to CV::MAT.
//	Mat dest(height, width, CV_32FC3, data);
//
//	//print the image
//	imwrite("subpic.jpg", dest);
//	Mat img3 = imread("subpic.jpg");
//	namedWindow("subpic", WINDOW_NORMAL);
//	resizeWindow("subpic", img3.cols, img3.rows);
//	imshow("subpic", img3);
//
//	//Makewhite and black only
//	for (int i = 0; i < (height * width) * 3; i += 3)
//	{
//
//		float r = data[i];
//		float g = data[i + 1];
//		float b = data[i + 2];
//		if (r >= v_threshold || g >= v_threshold || b >= v_threshold)
//		{
//			data[i] = 255;
//			data[i + 1] = 255;
//			data[i + 2] = 255;
//		}
//
//
//	}
//	Mat dest2(height, width, CV_32FC3, data);
//
//	//print the image
//	imwrite("blackandwhite.jpg", dest2);
//	Mat img4 = imread("blackandwhite.jpg");
//	namedWindow("blackandwhite", WINDOW_NORMAL);
//	resizeWindow("blackandwhite", img4.cols, img4.rows);
//	imshow("blackandwhite", img4);
//
//
//	waitKey(0);
//	return 0;
//}