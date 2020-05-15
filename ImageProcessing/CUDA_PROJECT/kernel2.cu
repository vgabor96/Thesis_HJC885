#include "cuda_runtime.h"
#include "device_launch_parameters.h"
#include <math.h>  
#include <io.h>
#include <iostream>
#include <fstream>
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


const int N = 1080; //768
const int M = 1920; //1024

int main()
{

	float v_threshold = 10;

	/*String folderpath = "C:\\Users\\loahc\\Documents\\GitHub\\Thesis_HJC885\\Unity\\Thesis_HJC885\\Assets\\screenshots\\train\\*.png";*/
	String folderpath = "C:\\Users\\loahc\\Documents\\GitHub\\Thesis_HJC885\\Unity\\Thesis_HJC885\\Assets\\screenshots\\test\\*.png";
		vector<String> filenames;
	cv::glob(folderpath, filenames);
	int hitcounter = 0;
	int nothitcounter = 0;
	printf("Picture Size N: %d \n", N);
	printf("Picture Size M: %d \n", M);
	int falsstring = 0;
	
	ofstream myfile;
	myfile.open("results.txt");
	//myfile.open("tests.txt");
	
	
	for (size_t x = 0; x < filenames.size()-1; x+=2)
	{
		Mat img = imread(filenames[x]);
		Mat img2 = imread(filenames[x+1]);


		unsigned first = filenames[x].find("(");
		unsigned last = filenames[x].find(")");
		string nametoattach = filenames[x].substr(first, last - (first-1));

		
		////SHOW IMAGE

		//namedWindow("pic_1", WINDOW_NORMAL);
		//resizeWindow("pic_1", img.cols, img.rows);
		//imshow("pic_1", img);

		//namedWindow("pic_2", WINDOW_NORMAL);
		//resizeWindow("pic_2", img2.cols, img2.rows);
		//imshow("pic_2", img2);




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

		//SHOW IAMGE
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
		//SHOW IMAGE
		//namedWindow("pic1_black", WINDOW_NORMAL);
		//resizeWindow("pic1_black", img4.cols, img4.rows);
		//imshow("pic1_black", img4);

		Mat src, src_gray;

		/// Read the image
		src = img4;

		if (!src.data)
		{
			return -1;
		}

		/// Convert it to gray
		cvtColor(src, src_gray, COLOR_RGB2GRAY);
		//SHOW IMAGE
		//namedWindow("Hough Circle Transform Gray", WINDOW_NORMAL);
		//imshow("Hough Circle Transform Gray", src);

		/// Reduce the noise so we avoid false circle detection
		GaussianBlur(src_gray, src_gray, Size(9,9), 2, 2); //Size(9, 9), 2, 2);
		////SHOW IMAGE
		//namedWindow("Hough Circle Transform Gauss", WINDOW_NORMAL);
		//imshow("Hough Circle Transform Gauss", src);

		vector<Vec3f> circles;
		

			/// Apply the Hough Transform to find the circles
			//		WorkingHoughCircles(src_gray, circles, HOUGH_GRADIENT, 1, src_gray.rows / 100, 10, 10, 0, 100);
			//HoughCircles(src_gray, circles, HOUGH_GRADIENT, 1,0.5, 10, 7, 0, 10);
			//HoughCircles(src_gray, circles, HOUGH_GRADIENT, 1,5, 10, 7, 1, 10);
		HoughCircles(src_gray, circles, HOUGH_GRADIENT, 1, src_gray.rows / 100, 10, 10, 0, 100);

			/// Draw the circles detected
			for (size_t i = 0; i < circles.size(); i++)
			{
				Point center(cvRound(circles[i][0]), cvRound(circles[i][1]));
				int radius = cvRound(circles[i][2]);
				// circle center
				circle(src, center, 2, Scalar(0, 255, 0), 2, 8, 0);
				// circle outline
				circle(src, center, radius, Scalar(0, 0, 255), 2, 8, 0);
				if (i > 0 && i< 2)
				{
					if (filenames[x].find("True") != std::string::npos) {
						falsstring = 1;
						hitcounter++;
					}
					else
					{
						falsstring = 0;
						nothitcounter++;
					}

					Point center1(cvRound(circles[i - 1][0]), cvRound(circles[i - 1][1]));
					printf("%i,", center1.x);
					printf("%i,", center1.y);
				

					Point center2(cvRound(circles[i][0]), cvRound(circles[i][1]));

					printf("%i,", center2.x);
					printf("%i,", center2.y);
					printf("%i\n",falsstring);
					myfile << nametoattach<< "\t" << center1.x << "\t" << center1.y <<"\t"<< center2.x << "\t" << center2.y<<"\t"<<falsstring<<"\n";
					line(src, center1, center2, Scalar(0, 0, 255), 2, 8, 0);
				}
			}
		


		/// Show your results
		namedWindow("Hough Circle Transform", WINDOW_NORMAL);
		imshow("Hough Circle Transform", src);

		//WAIT OR NOT
		waitKey(0);
	}



	printf("Number of Hits: %i\n", hitcounter);
	printf("Number of Non-Hits: %i\n", nothitcounter);
	myfile.close();
	return 0;
}