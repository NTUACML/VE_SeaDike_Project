#pragma once
#include <iostream>
#include <fstream>
#include "Module2_Var.h"

class Module2_Internal
{
public:
	//Constructor
	Module2_Internal();

	//Distructor
	virtual ~Module2_Internal();

	//Public Func
	void SetVar(Module2_Var *_Var);

	//Cal Function
	bool GeoPreCal();
	bool WeightCal();
	bool EarthQuakeForceCal();

private:
	Module2_Var *Var;

};

