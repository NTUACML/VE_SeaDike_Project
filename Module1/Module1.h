// Module1.h

#pragma once
#include "include\Module1_Internal.h"
#include "include\Module1_Var.h"
#include <msclr/marshal_cppstd.h> 
#include <string>
using namespace System;

namespace VE_SD {
	public value struct MyPoint
	{
		int x, y, z, time;
		MyPoint(int x, int y, int z, int t)
		{
			this->x = x;
			this->y = y;
			this->z = z;
			this->time = t;
		}
	};

	public ref class Module1
	{
	public:
		//Constructor
		Module1();

		//Distructor
		~Module1();
		!Module1();


		//Public Variable
		String^ ErrMsg;

		//Public Function
		//- Block Set
		int NewBlock(double _Density, double _FrictionC); 
		bool DeleteBlock(int NumOfBlock);
		bool SetBlockCoord(int NumOfBlock, double x, double y);
		int GetNumOfBlock();
		bool DeleteAllBlockData();

		//- Level Set
		int NewLevel(double _EL);
		bool DeleteAllLevel();

		//- Compute Var Input API
		bool WaterDesignInput(double _H0, double _HWL, double _DensitySea);
		bool WaveDesignInput(int _Direction, double _T0, double _Kr,
							double _Ks, double _Kd, double _lamda, 
							double _beta);
		bool BaseDesignInput(double _S, double _Base_Level, double _Breaker_Level);
		bool SF_CoefInput(double _SlideSF, double _RotateSF);

		//- Run API
		bool Run(); //Run Main Check Processor
		bool OutPutLogFile(String ^ Pois);

		//- Test
		void Test();
		void VarOut(double % out);

		MyPoint AA; //Test

	private:
		//Data
		Module1_Internal *Internal;
		Module1_Var *Var;

		//Func
		void MsgAdd();
	};
}
