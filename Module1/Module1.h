// Module1.h

#pragma once
#include "include\Module1_Internal.h"
#include "include\Module1_Var.h"

using namespace System;

namespace VE_SD {

	public ref class Module1
	{
	public:
		//Constructor
		Module1();

		//Distructor
		~Module1();

		//Public Variable
		String^ ErrMsg;

		//Public Function
		//- Block Set
		int NewBlock(double _Density, double _FrictionC); 
		bool DeleteBlock(int NumOfBlock);
		bool SetBlockCoord(int NumofBlock, double x, double y);

	private:
		Module1_Internal *Internal;
		Module1_Var *Var;
	};
}
