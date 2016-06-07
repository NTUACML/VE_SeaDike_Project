// �o�O�D�n DLL �ɮסC

#include "stdafx.h"
#include "Module1.h"
#include <fstream>

VE_SD::Module1::Module1():Var(NULL),Internal(NULL)
{
	//Initial Obj
	Var = new Module1_Var();
	Internal = new Module1_Internal();
	//Get Variable
	Internal->SetVar(Var);

	//Mesg
	ErrMsg += "*** Module - 1 �p��}�l *** \r\n";
}

VE_SD::Module1::~Module1()
{
	if (Internal != NULL) delete Internal;
	if (Var != NULL) delete Var;

}

VE_SD::Module1::!Module1()
{
	this->~Module1();
}

int VE_SD::Module1::NewBlock(double _Density, double _FrictionC, bool _CalMoment) {
	Var->BlockData.emplace_back(_Density,_FrictionC, _CalMoment);
	return int(Var->BlockData.size());
}

bool VE_SD::Module1::DeleteBlock(int NumOfBlock){
	if (Var->BlockData.size() != 0 && NumOfBlock <= int(Var->BlockData.size())) {
		int Id = NumOfBlock - 1;
		Var->BlockData.erase(Var->BlockData.begin() + Id);
		return true;
	}
	else {
		ErrMsg += ("Wrong Delete Block Id !(In Delete Block " + NumOfBlock.ToString() + ") \n");
		return false;
	}	
}

bool VE_SD::Module1::SetBlockCoord(int NumOfBlock, double x, double y){
	if(Var->BlockData.size()!=0 && NumOfBlock <= int(Var->BlockData.size())){
		int Id = NumOfBlock - 1;
		Var->BlockData[Id].Node.emplace_back(x, y);
		return true;
	}
	else {
		ErrMsg += "Wrong Delete Block Id (In Set Block Coord)! \n";
		return false;
	}
		
}

int VE_SD::Module1::GetNumOfBlock()
{
	return int(Var->BlockData.size());
}

bool VE_SD::Module1::DeleteAllBlockData()
{
	Var->BlockData.clear();
	if (GetNumOfBlock() == 0) {
		return true;
	}
	else {
		return false;
	}
	
}

int VE_SD::Module1::NewLevel(double _EL)
{
	Var->LevelSection.emplace_back(_EL);
	return int(Var->LevelSection.size());
}

bool VE_SD::Module1::DeleteAllLevel()
{
	Var->LevelSection.clear();
	return true;
}

bool VE_SD::Module1::WaterDesignInput(double _H0, double _HWL, double _DensitySea)
{
	Var->H0 = _H0;
	Var->HWL = _HWL;
	Var->DensitySea = _DensitySea;
	return true;
}

bool VE_SD::Module1::WaveDesignInput(int _Direction, double _T0, double _Kr, 
									double _Ks, double _Kd, double _lamda,
									double _beta)
{
	Var->Direction = _Direction;
	Var->T0 = _T0;
	Var->Kr = _Kr;
	Var->Ks = _Ks;
	Var->Kd = _Kd;
	Var->lamda = _lamda;
	// Deg to Rad (beta)

	Var->beta = _beta * (M_PI / 180.0);
	return true;
}

bool VE_SD::Module1::BaseDesignInput(double _S, double _Base_Level, double _Breaker_Level)
{
	Var->S = _S;
	Var->Base_Level = _Base_Level;
	Var->Breaker_Level = _Breaker_Level;
	return true;
}

bool VE_SD::Module1::OptionalVarInput(double _hb)
{
	Var->hb = _hb;
	return true;
}

bool VE_SD::Module1::SF_CoefInput(double _SlideSF, double _RotateSF)
{
	Var->SlideSF = _SlideSF;
	Var->RotateSF = _RotateSF;
	return true;
}

bool VE_SD::Module1::WaveBreakOutsideCondition(bool _WaveBreakFuncOutside)
{
	Var->WaveBreakFuncOutside = _WaveBreakFuncOutside;
	return true;
}

bool VE_SD::Module1::WaveBreakInsideCondition(bool _WaveBreakFuncInside)
{
	Var->WaveBreakFuncInside = _WaveBreakFuncInside;
	return true;
}

bool VE_SD::Module1::WaveBreakUpsideCondition(bool _WaveBreakFuncUpside)
{
	Var->WaveBreakFuncUpside = _WaveBreakFuncUpside;
	return true;
}

bool VE_SD::Module1::WaveBreakOutsideInput(double _Density, double _Coef, double _Slope)
{
	Var->DensityOutside = _Density;
	Var->SafeCoefOutside = _Coef;
	Var->SlopeOutside = _Slope;
	return true;
}

bool VE_SD::Module1::WaveBreakInsideInput(double _Density, double _Coef, double _Slope, double _Kt)
{
	Var->DensityInside = _Density;
	Var->SafeCoefInside = _Coef;
	Var->SlopeInside = _Slope;
	Var->Kt = _Kt;
	return true;
}

bool VE_SD::Module1::WaveBreakUpsideInput(double _Density, double _Coef, double _Slope)
{
	Var->DensityUpside = _Density;
	Var->SafeCoefUpside = _Coef;
	Var->SlopeUpside = _Slope;
	return true;
}

bool VE_SD::Module1::UpperBlockCheckCondition(bool _UpperBlockCheckCondi)
{
	Var->UpperBlockCheckCondi = _UpperBlockCheckCondi;
	return true;
}

bool VE_SD::Module1::UpperBlockCheckInput(double _Vc, double _Bk_plun, double _Up_EL)
{
	Var->Vc = _Vc;
	Var->Bk_plun = _Bk_plun;
	Var->UpBlockEL = _Up_EL;
	return true;
}

bool VE_SD::Module1::Get_DataBank_Data()
{
	// EL Section!
	VarBank.EL_Out = gcnew array< EL_SectionResult >(int(Var->LevelSection.size()));
	for (int i = 0; i < Var->LevelSection.size(); ++i) {
		VarBank.EL_Out[i].EL = Var->LevelSection[i].Level;
		VarBank.EL_Out[i].P = Var->LevelSection[i].P;
		VarBank.EL_Out[i].FP = Var->LevelSection[i].FP;
		VarBank.EL_Out[i].Y = Var->LevelSection[i].L_Y;
		VarBank.EL_Out[i].Mp = Var->LevelSection[i].Mp;

		VarBank.EL_Out[i].BlockNum = gcnew array< Int32 >(int(Var->LevelSection[i].BlockId.size()));
		for (int j = 0; j < Var->LevelSection[i].BlockId.size(); ++j) {
			VarBank.EL_Out[i].BlockNum[j] = int(Var->LevelSection[i].BlockId[j]);
		}
	}

	// EL Block!
	VarBank.Block_Out = gcnew array< BlockResult >(int(Var->BlockData.size()));
	for (int i = 0; i < Var->BlockData.size(); ++i) {
		VarBank.Block_Out[i].A = Var->BlockData[i].Area;
		VarBank.Block_Out[i].garma = Var->BlockData[i].Density;
		VarBank.Block_Out[i].W = Var->BlockData[i].SelfWeight;
		VarBank.Block_Out[i].X = Var->BlockData[i].WeightC.x;
		VarBank.Block_Out[i].Mw = Var->BlockData[i].Mw;
	}

	// Var Get!
	VarBank.h = Var->h;
	VarBank.h_plun = Var->h_plun;
	VarBank.hc = Var->hc;
	VarBank.d = Var->d;
	VarBank.L0 = Var->L0;
	VarBank.H0_plun = Var->H0_plun;
	VarBank.L = Var->L;
	VarBank.h_D_L0 = Var->h_D_L0;
	VarBank.beta0 = Var->beta0;
	VarBank.beta1 = Var->beta1;
	VarBank.betaMax = Var->betaMax;
	VarBank.beta0_Star = Var->beta0_Star;
	VarBank.beta1_Star = Var->beta1_Star;
	VarBank.betaMax_Star = Var->betaMax_Star;
	VarBank.Hs = Var->Hs;
	VarBank.Hmax = Var->Hmax;
	VarBank.hb = Var->hb;
	VarBank.alpha1 = Var->alpha1;
	VarBank.alpha2 = Var->alpha2;
	VarBank.alpha3 = Var->alpha3;
	VarBank.eta_Star = Var->eta_Star;
	VarBank.hc_Star = Var->hc_Star;
	VarBank.P1 = Var->P1;
	VarBank.P2 = Var->P2;
	VarBank.P3 = Var->P3;
	VarBank.P4 = Var->P4;
	VarBank.Pu = Var->Pu;
	VarBank.Fu = Var->Fu;
	VarBank.Mu = Var->Mu;
	VarBank.CalBody_SlideSF = Var->CalBody_SlideSF;
	VarBank.CalBody_RotateSF = Var->CalBody_RotateSF;
	VarBank.W = Var->W;
	VarBank.Mw = Var->Mw;
	VarBank.Mp = Var->Mp;
	VarBank.W1 = Var->W1;
	VarBank.W2 = Var->W2;
	VarBank.W3 = Var->W3;
	VarBank.CalUpper_SlideSF = Var->CalUpper_SlideSF;
	VarBank.CalUpper_RotateSF = Var->CalUpper_RotateSF;
	VarBank.CalBk = Var->Bk;
	VarBank.C = Var->C;
	VarBank.CentAngle = Var->CentAngle;
	VarBank.Nc = Var->Nc;
	VarBank.Nq = Var->Nq;
	VarBank.Nr = Var->Nr;
	VarBank.V = Var->V;
	VarBank.H = Var->H;
	VarBank.Mr = Var->Mr;
	VarBank.Mo = Var->Mo;
	VarBank.BaseDen = Var->BaseDen;
	VarBank.U = Var->U;
	VarBank.D = Var->D;
	VarBank.BaseFS = Var->BaseFS;
	VarBank.B_6 = Var->B_6;
	VarBank.C_x = Var->C_x;
	VarBank.e_x = Var->e_x;
	VarBank.B_plum = Var->B_plum;
	VarBank.Df = Var->Df;
	VarBank.Base_P1 = Var->Base_P1;
	VarBank.Base_P2 = Var->Base_P2;
	VarBank.Base_Theta = Var->Base_Theta;
	VarBank.B_plum2 = Var->B_plum2;
	VarBank.R1 = Var->R1;
	VarBank.R2 = Var->R2;
	VarBank.Qu = Var->Qu;
	VarBank.Qa = Var->Qa;

	return true;
}

bool VE_SD::Module1::Run()
{
	// Geo Pre-Calculate
	if (!Internal->GeoPreCal() // Geo Pre-Calculate
		||
		!Internal->WaterLevelCal() // Water Level Cal
		||
		!Internal->WavePressureCal() // Wave Pressure Moment Cal
		||
		!Internal->WeightCal() // Self Weight Moment Cal
		||
		!Internal->BodySafeCheck() // Safe Check!!!!!
		||
		!Internal->BreakerSafeCheck() // Breaker Safe Check
		||
		!Internal->UpperSafeCheck() //Upper Block Safe Check
		||
		!Internal->BasementSafeCheck() // Basement Safe Check
		) {
		MsgAdd();
		ErrMsg += "*** Module - 1 �p�⥢�� *** \r\n";
	}

	// Mesg Print
	MsgAdd();

	//Mesg
	ErrMsg += "*** Module - 1 �p�⵲�� *** \r\n";

	//Test---

	return true;
}

bool VE_SD::Module1::OutPutLogFile(String ^ Pois)
{
	//- File Open
	std::ofstream FILE;
	std::string C_str = msclr::interop::marshal_as<std::string>(Pois);
	FILE.open(C_str);
	//- Out Contents
	FILE << "******�I���Ѽ�******"<< std::endl;
	FILE << "������V: ";
	if (Var->Direction == 1)
	{
		FILE << "�k" << std::endl;
	}
	else {
		FILE << "��" << std::endl;
	}
	FILE << "�i��: " << Var->H0 << std::endl;
	FILE << "�g��: " << Var->T0 << std::endl;
	FILE << "���: " << Var->HWL << std::endl;
	FILE << "�Y��: " << Var->S << std::endl;
	FILE << "�a���u: " << Var->Base_Level << std::endl;
	FILE << "���i�����h: " << Var->Breaker_Level << std::endl;
	FILE << "��g�Y��: " << Var->Kr << std::endl;
	FILE << "�L�ƫY��: " << Var->Ks << std::endl;
	FILE << "¶�g�Y��: " << Var->Kd << std::endl;
	FILE << "���Y��: " << Var->lamda << std::endl;
	FILE << "���u����: " << Var->beta << std::endl;
	FILE << "�����K��: " << Var->DensitySea << std::endl;
	FILE << "******�w���Y��******" << std::endl;
	FILE << "�ư�SF: " << Var->SlideSF << std::endl;
	FILE << "�ɭ�SF: " << Var->RotateSF << std::endl;
	FILE << "******�X��϶�******" << std::endl;
	FILE << "�`�϶���: " << Var->BlockData.size() << std::endl;
	FILE << "�̤jLevel: " << Var->Max_level << std::endl;
	FILE << "�̤pLevel: " << Var->Min_level << std::endl;
	FILE << "��ک��ɼe: " << Var->B << std::endl;
	FILE <<  std::endl;
	for (size_t i = 0; i < Var->BlockData.size(); i++)
	{
		FILE << "�϶��椸 " << i+1 <<" :"<< std::endl;
		FILE << "�϶��K��: " << Var->BlockData[i].Density << std::endl;
		FILE << "�϶�����: " << Var->BlockData[i].FrictionC << std::endl;
		FILE << "----�`�I�y��----" << std::endl;
		for (auto & NodeElement : Var->BlockData[i].Node) {
			FILE << "X: " << NodeElement.x << "\t" << "Y: " << NodeElement.y << std::endl;
		}
		FILE << std::endl;
	}
	FILE << "******EL����******" << std::endl;
	for (size_t i = 0; i < Var->LevelSection.size(); i++)
	{
		FILE << "EL " << i + 1 << " : " << Var->LevelSection[i].Level<< std::endl;
		FILE << "-> �]�t�϶��椸�s��: ";
		for (size_t j = 0; j < Var->LevelSection[i].BlockId.size(); j++)
		{
			FILE << Var->LevelSection[i].BlockId[j] + 1<< " ";
		}
		FILE << std::endl;
	}

	FILE << "******���`����******" << std::endl;
	FILE << "h: " << Var->h << std::endl;
	FILE << "h\': " << Var->h_plun << std::endl;
	FILE << "hc: " << Var->hc << std::endl;
	FILE << "d: " << Var->d << std::endl;
	FILE << "L0: " << Var->L0 << std::endl;
	FILE << "H0\': " << Var->H0_plun << std::endl;
	FILE << "L: " << Var->L << std::endl;
	FILE << "h/L0: " << Var->h_D_L0 << std::endl;
	FILE << "hb: " << Var->hb << std::endl;
	FILE << "******�i���p��******" << std::endl;
	FILE << "Beta 0: " << Var->beta0 << std::endl;
	FILE << "Beta 1: " << Var->beta1 << std::endl;
	FILE << "Beta Max: " << Var->betaMax << std::endl;
	FILE << "Beta 0*: " << Var->beta0_Star << std::endl;
	FILE << "Beta 1*: " << Var->beta1_Star << std::endl;
	FILE << "Beta Max*: " << Var->betaMax_Star << std::endl;
	FILE << "Hs: " << Var->Hs << std::endl;
	FILE << "Hmax: " << Var->Hmax << std::endl;
	FILE << "******�i���p��******" << std::endl;
	FILE << "Alpha 1: " << Var->alpha1 << std::endl;
	FILE << "Alpha 2: " << Var->alpha2 << std::endl;
	FILE << "Alpha 3: " << Var->alpha3 << std::endl;
	FILE << "Eta*: " << Var->eta_Star << std::endl;
	FILE << "hc*: " << Var->hc_Star << std::endl;
	FILE << "P1: " << Var->P1 << std::endl;
	FILE << "P2: " << Var->P2 << std::endl;
	FILE << "P3: " << Var->P3 << std::endl;
	FILE << "P4: " << Var->P4 << std::endl;
	FILE << "******�i���s�x******" << std::endl;
	for (size_t i = 0; i < Var->LevelSection.size(); i++)
	{
		FILE << "EL :" << Var->LevelSection[i].Level<< "\t" <<
			"P: "<< Var->LevelSection[i].P << "\t" <<
			"FP: "<< Var->LevelSection[i].FP << "\t" <<
			"Y: " << Var->LevelSection[i].L_Y << "\t" <<
			"MP: " << Var->LevelSection[i].Mp << "\t" <<
			 std::endl;
	}
	FILE << "�i���X�p: " << Var->Fp << std::endl;
	FILE << "�ɭ��s�x: " << Var->Mp << std::endl;
	FILE << "******�����O******" << std::endl;
	FILE << "Pu: " << Var->Pu << std::endl;
	FILE << "Fu: " << Var->Fu << std::endl;
	FILE << "Mu: " << Var->Mu << std::endl;
	FILE << "******���حp��******" << std::endl;
	size_t id;
	for (size_t i = 0; i < Var->LevelSection.size(); i++)
	{
		FILE << "EL " << " : " << Var->LevelSection[i].Level << std::endl;
		FILE << "-> �]�t�϶��s���P��T: " << std::endl;
		
		for (size_t j = 0; j < Var->LevelSection[i].BlockId.size(); j++)
		{
			id = Var->LevelSection[i].BlockId[j];
			FILE << "Id: "<< id + 1 <<"\t";
			FILE << "A: " << Var->BlockData[id].Area << "\t";
			FILE << "Garma: " << Var->BlockData[id].Density << "\t";
			FILE << "W: " << Var->BlockData[id].SelfWeight << "\t";
			FILE << "X: " << Var->BlockData[id].WeightC.x << "\t";
			FILE << "Mw: " << Var->BlockData[id].Mw;
			FILE << std::endl;
		}
	}
	FILE << "�����`��: " << Var->W << std::endl;
	FILE << "�`��O�x: " << Var->Mw << std::endl;
	FILE << "******����w�w�ˮ�******" << std::endl;
	FILE << "����ư�SF: " << Var->CalBody_SlideSF << std::endl;
	FILE << "����ɭ�SF: " << Var->CalBody_RotateSF << std::endl;

	if (Var->WaveBreakFuncOutside == true) {
		FILE << "******���i�u�{-��~���p��******" << std::endl;
		FILE << "W1: " << Var->W1 << std::endl;
	}

	if (Var->WaveBreakFuncUpside == true) {
		FILE << "******���i�u�{-���Y���p��******" << std::endl;
		FILE << "W2: " << Var->W2 << std::endl;
	}

	if (Var->WaveBreakFuncInside == true) {
		FILE << "******���i�u�{-���Y���p��******" << std::endl;
		FILE << "W3: " << Var->W3 << std::endl;
	}

	if (Var->UpperBlockCheckCondi == true) {
		FILE << "******���𳡦w�w�ˮ�******" << std::endl;
		FILE << "����ư�SF: " << Var->CalUpper_SlideSF << std::endl;
		FILE << "����ɭ�SF: " << Var->CalUpper_RotateSF << std::endl;
		FILE << "�V�d�g�e�\�����O: " << Var->Vc << std::endl;
		FILE << "�p��BK: " << Var->Bk << std::endl;
	}
	FILE.close();
	return true;
}

void VE_SD::Module1::MsgAdd()
{
	ErrMsg += gcnew String(Var->Err_Msg.c_str());
	Var->Err_Msg.clear();
}

bool VE_SD::Module1::BasementCheckCondition(bool _BasementCheckCondi) {
	Var->BasementCheckCondi = _BasementCheckCondi;
	return true;
}
bool VE_SD::Module1::BasementCheckInput(double _C, double _CentAngle, double _Nc, double _Nq, double _Nr,
	double _BaseDen, double _U, double _D, double _BaseFS) {
	Var->C = _C;
	Var->CentAngle = _CentAngle * (M_PI / 180.0); // To Rad
	Var->Nc = _Nc;
	Var->Nq = _Nq;
	Var->Nr = _Nr;
	Var->BaseDen = _BaseDen;
	Var->U = _U;
	Var->D = _D;
	Var->BaseFS = _BaseFS;
	return true;
}
