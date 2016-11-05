// Module2.h

#pragma once
#include <msclr/marshal_cppstd.h> 
#include <string>
#include <exception>
# define M_PI 3.14159265358979323846  /* pi */

using namespace System;

namespace VE_SD {

	public ref class Module2
	{
		//Constructor
		Module2();

		//Distructor
		~Module2();
		!Module2();

		//Public Variable
		String^ ErrMsg;

		//Public Variable// TODO:  在此加入這個類別的方法。
	};
}
