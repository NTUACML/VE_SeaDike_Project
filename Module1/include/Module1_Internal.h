#pragma once
#include "Module1_Var.h"
#include <vector>
#include <fstream>
#include <string>

class Module1_Internal
{
public:
	//Constructor
	Module1_Internal();

	//Distructor
	~Module1_Internal();

	//Public Func
	void SetVar(Module1_Var *_Var);

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

