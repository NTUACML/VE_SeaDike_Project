#pragma once
#include "Module1_Var.h"
#include <iostream>
#include <vector>
#include <fstream>
#include <string>
#include <cmath>
#include <algorithm>
# define M_PI 3.14159265358979323846  /* pi */

class Module1_Internal
{
public:
	//Constructor
	Module1_Internal();

	//Distructor
	~Module1_Internal();

	//Public Func
	void SetVar(Module1_Var *_Var);

	//Cal Function
	bool GeoPreCal();
	bool WaterLevelCal();
	bool WavePressureCal();

	//- Test Out
	template<typename T>
	void TestFileOut(std::string FileName,T Variable){
		std::ofstream FILE;
		FILE.open(FileName);
		FILE << Variable << std::endl;
		FILE.close();
	}
private:
	Module1_Var *Var;
};

