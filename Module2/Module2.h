// Module2.h

#pragma once
#include "include\Module2_Internal.h"
#include "include\Module2_Var.h"
#include <msclr/marshal_cppstd.h> 
#include <string>
#include <exception>
# define M_PI 3.14159265358979323846  /* pi */

using namespace System;

namespace VE_SD {

	public ref class Module2
	{
	public:
		// Constructor
		Module2();

		// Distructor

		~Module2();
		!Module2();

		// Public Variable
		String^ ErrMsg;

		// Public Function
		//- Compute Var Input API
		void WaterDesignInput(double _HWL, double _LWL);
		void ForceDesignInput(double _Q, double _Qe, double _Ta);
		void EarthquakeDesignInput(double _K, double _K_plun);
		void MaterialDesignInput(double _InnerPhi, double _WallPhi, double _Beta);
		void BaseDesignInput(double _U, double _D, double _BasePhi, double _C);
		void MF_DesignInput(double _Nq, double _Nr, double _Nc);
		void SF_CoefInput(double _SlideSF, double _RotateSF, double _BaseSF);

		//- Block Set
		int NewBlock(double _Density, double _FrictionC, bool _CalMoment);
		bool DeleteBlock(int NumOfBlock);
		bool SetBlockCoord(int NumOfBlock, double x, double y);
		int GetNumOfBlock();
		bool DeleteAllBlockData();

	private:
		//Data
		Module2_Internal *Internal;
		Module2_Var *Var;
	};
}
