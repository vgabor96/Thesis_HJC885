#include "cuda_runtime.h"

__global__ void WarmUpKernel()
{
	return;
}

void WarmUp()
{
	WarmUpKernel << <100, 32 >> > ();
}