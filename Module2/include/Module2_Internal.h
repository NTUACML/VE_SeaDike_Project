#pragma once
#include <iostream>
#include <fstream>
#include "Module2_Var.h"
# define M_PI 3.14159265358979323846  /* pi */
# define M_G 9.81  /* g */

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
	bool HorizontalSoilForceCal();

private:
	Module2_Var *Var;

};

