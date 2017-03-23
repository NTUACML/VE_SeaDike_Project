// 這是主要 DLL 檔案。

#include "Module2.h"

VE_SD::Module2::Module2():Var(NULL), Internal(NULL)
{
	//Initial Obj
	Var = new Module2_Var();
	Internal = new Module2_Internal();
	//Get Variable
	Internal->SetVar(Var);

	//Mesg
	ErrMsg += "*** Module - 2 計算開始 *** \r\n";
}

VE_SD::Module2::~Module2()
{
	if (Internal != NULL) delete Internal;
	if (Var != NULL) delete Var;
}

VE_SD::Module2::!Module2()
{
	this->~Module2(); 
}

void VE_SD::Module2::WaterDesignInput(double _HWL, double _LWL, double _RWL)
{
	Var->HWL = _HWL;
	Var->LWL = _LWL;
	Var->RWL = _RWL;
}

void VE_SD::Module2::ForceDesignInput(double _Q, double _Qe, double _Ta)
{
	Var->Q = _Q;
	Var->Qe = _Qe;
	Var->Ta = _Ta;
}

void VE_SD::Module2::EarthquakeDesignInput(double _K, double _K_plun)
{
	Var->K = _K;
	Var->K_plun = _K_plun;
}

void VE_SD::Module2::MaterialDesignInput(double _InnerPhi, double _WallPhi, double _Beta, double _hd)
{
	Var->InnerPhi = _InnerPhi;
	Var->WallPhi = _WallPhi;
	Var->Beta = _Beta;
	Var->hd = _hd;
}

void VE_SD::Module2::BaseDesignInput(double _U, double _D, double _BasePhi, double _C, double _soilR_Earth, double _soilR_Water, double _rw)
{
	Var->U = _U;
	Var->D = _D;
	Var->BasePhi = _BasePhi;
	Var->C = _C;
	Var->soilR_Earth = _soilR_Earth;
	Var->soilR_Water = _soilR_Water;
	Var->rw = _rw;
}

void VE_SD::Module2::MF_DesignInput(double _Nq, double _Nr, double _Nc)
{
	Var->Nq = _Nq;
	Var->Nr = _Nr;
	Var->Nc = _Nc;
}

void VE_SD::Module2::SF_CoefInput(double _SlideSF, double _RotateSF, double _BaseSF)
{
	Var->SlideSF = _SlideSF;
	Var->RotateSF = _RotateSF;
	Var->BaseSF = _BaseSF;
}
//新增地震時安全係數
void VE_SD::Module2::SF_CoefInput_E(double _SlideSF_E, double _RotateSF_E, double _BaseSF_E)
{
	Var->SlideSF_E = _SlideSF_E;
	Var->RotateSF_E = _RotateSF_E;
	Var->BaseSF_E = _BaseSF_E;
}

void VE_SD::Module2::KaInput(double _ka, double _ka_17, double _ka_33)
{
	Var->ka = _ka;
	Var->ka_17 = _ka_17;
	Var->ka_33 = _ka_33;
}

int VE_SD::Module2::NewBlock(double _Density, double _EQ_Density, double _FrictionC, bool _CalMoment) {
	Var->BlockData.emplace_back(_Density, _EQ_Density, _FrictionC, _CalMoment);
	return int(Var->BlockData.size());
}

bool VE_SD::Module2::DeleteBlock(int NumOfBlock) {
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

bool VE_SD::Module2::SetBlockCoord(int NumOfBlock, double x, double y) {
	if (Var->BlockData.size() != 0 && NumOfBlock <= int(Var->BlockData.size())) {
		int Id = NumOfBlock - 1;
		Var->BlockData[Id].Node.emplace_back(x, y);
		return true;
	}
	else {
		ErrMsg += "Wrong Delete Block Id (In Set Block Coord)! \n";
		return false;
	}
}

int VE_SD::Module2::GetNumOfBlock()
{
	return int(Var->BlockData.size());
}

bool VE_SD::Module2::DeleteAllBlockData()
{
	Var->BlockData.clear();
	if (GetNumOfBlock() == 0) {
		return true;
	}
	else {
		return false;
	}
}

int VE_SD::Module2::NewLevel(double _EL)
{
	Var->LevelSection.emplace_back(_EL);
	return int(Var->LevelSection.size());
}

bool VE_SD::Module2::DeleteAllLevel()
{
	Var->LevelSection.clear();
	return true;
}

bool VE_SD::Module2::Get_DataBank_Data(){

	VarBank.EL_Out = gcnew array< EL_SectionResult >(int(Var->LevelSection.size()));
	for (int i = 0; i < Var->LevelSection.size(); ++i) {
		VarBank.EL_Out[i].EL = Var->LevelSection[i].Level;
		VarBank.EL_Out[i].Level_sum_W = Var->LevelSection[i].Level_sum_W;
		VarBank.EL_Out[i].Level_sum_Mx = Var->LevelSection[i].Level_sum_Mx;
		VarBank.EL_Out[i].Level_total_arm = Var->LevelSection[i].Level_total_arm;
		VarBank.EL_Out[i].pre_sum_W = Var->LevelSection[i].pre_sum_W;
		VarBank.EL_Out[i].pre_sum_Mx = Var->LevelSection[i].pre_sum_Mx;
		VarBank.EL_Out[i].pre_total_arm = Var->LevelSection[i].pre_total_arm;
		VarBank.EL_Out[i].Level_sum_WE = Var->LevelSection[i].Level_sum_WE;
		VarBank.EL_Out[i].Level_sum_MxE = Var->LevelSection[i].Level_sum_MxE;
		VarBank.EL_Out[i].Level_total_armE = Var->LevelSection[i].Level_total_armE;
		VarBank.EL_Out[i].pre_sum_WE = Var->LevelSection[i].pre_sum_WE;
		VarBank.EL_Out[i].pre_sum_MxE = Var->LevelSection[i].pre_sum_MxE;
		VarBank.EL_Out[i].pre_total_armE = Var->LevelSection[i].pre_total_armE;

		VarBank.EL_Out[i].Fh= Var->LevelSection[i].Fh;
		VarBank.EL_Out[i].Fh_y = Var->LevelSection[i].Fh_y;
		VarBank.EL_Out[i].Fh_Mh = Var->LevelSection[i].Fh_Mh;
		VarBank.EL_Out[i].Level_sum_Fh = Var->LevelSection[i].Level_sum_Fh;
		VarBank.EL_Out[i].Level_sum_FhMh = Var->LevelSection[i].Level_sum_FhMh;
		VarBank.EL_Out[i].Level_total_Fhy = Var->LevelSection[i].Level_total_Fhy;
		VarBank.EL_Out[i].pre_sum_Fh = Var->LevelSection[i].pre_sum_Fh;
		VarBank.EL_Out[i].pre_sum_FhMh = Var->LevelSection[i].pre_sum_FhMh;
		VarBank.EL_Out[i].pre_total_Fhy = Var->LevelSection[i].pre_total_Fhy;
		VarBank.EL_Out[i].Fh_E = Var->LevelSection[i].Fh_E;
		VarBank.EL_Out[i].Fh_y_E = Var->LevelSection[i].Fh_y_E;
		VarBank.EL_Out[i].Fh_Mh_E = Var->LevelSection[i].Fh_Mh_E;
		VarBank.EL_Out[i].Level_sum_Fh_E = Var->LevelSection[i].Level_sum_Fh_E;
		VarBank.EL_Out[i].Level_sum_FhMh_E = Var->LevelSection[i].Level_sum_FhMh_E;
		VarBank.EL_Out[i].Level_total_Fhy_E = Var->LevelSection[i].Level_total_Fhy_E;
		VarBank.EL_Out[i].pre_sum_Fh_E = Var->LevelSection[i].pre_sum_Fh_E;
		VarBank.EL_Out[i].pre_sum_FhMh_E = Var->LevelSection[i].pre_sum_FhMh_E;
		VarBank.EL_Out[i].pre_total_Fhy_E = Var->LevelSection[i].pre_total_Fhy_E;

		VarBank.EL_Out[i].Level_sum_Fv = Var->LevelSection[i].Level_sum_Fv;
		VarBank.EL_Out[i].Level_sum_FvMv = Var->LevelSection[i].Level_sum_FvMv;
		VarBank.EL_Out[i].Level_total_Fvx = Var->LevelSection[i].Level_total_Fvx;
		VarBank.EL_Out[i].Level_sum_Fv_E = Var->LevelSection[i].Level_sum_Fv_E;
		VarBank.EL_Out[i].Level_sum_FvMv_E = Var->LevelSection[i].Level_sum_FvMv_E;
		VarBank.EL_Out[i].Level_total_Fvx_E = Var->LevelSection[i].Level_total_Fvx_E;

		VarBank.EL_Out[i].Fw_sum = Var->LevelSection[i].Fw_sum;
		VarBank.EL_Out[i].Fw_y = Var->LevelSection[i].Fw_y;
		VarBank.EL_Out[i].Fw_Mw_sum = Var->LevelSection[i].Fw_Mw_sum;
		VarBank.EL_Out[i].Level_sum_Fw = Var->LevelSection[i].Level_sum_Fw;
		VarBank.EL_Out[i].Level_sum_FwMw = Var->LevelSection[i].Level_sum_FwMw;
		VarBank.EL_Out[i].Level_total_Fwy = Var->LevelSection[i].Level_total_Fwy;
		VarBank.EL_Out[i].pre_sum_Fw = Var->LevelSection[i].pre_sum_Fw;
		VarBank.EL_Out[i].pre_sum_FwMw = Var->LevelSection[i].pre_sum_FwMw;
		VarBank.EL_Out[i].pre_total_Fwy = Var->LevelSection[i].pre_total_Fwy;

		VarBank.EL_Out[i].Ft_y = Var->LevelSection[i].Ft_y;
		VarBank.EL_Out[i].Ft_Mt = Var->LevelSection[i].Ft_Mt;

		VarBank.EL_Out[i].VForcesum = Var->LevelSection[i].VForcesum;
		VarBank.EL_Out[i].VMomentsum = Var->LevelSection[i].VMomentsum;
		VarBank.EL_Out[i].VForcesum_E = Var->LevelSection[i].VForcesum_E;
		VarBank.EL_Out[i].VMomentsum_E = Var->LevelSection[i].VMomentsum_E;
		VarBank.EL_Out[i].HForcesum = Var->LevelSection[i].HForcesum;
		VarBank.EL_Out[i].HMomentsum = Var->LevelSection[i].HMomentsum;
		VarBank.EL_Out[i].HForcesum_E = Var->LevelSection[i].HForcesum_E;
		VarBank.EL_Out[i].HMomentsum_E = Var->LevelSection[i].HMomentsum_E;

		VarBank.EL_Out[i].SF_slide = Var->LevelSection[i].SF_slide;
		VarBank.EL_Out[i].SF_slide_E = Var->LevelSection[i].SF_slide_E;
		VarBank.EL_Out[i].SF_overturning = Var->LevelSection[i].SF_overturning;
		VarBank.EL_Out[i].SF_overturning_E = Var->LevelSection[i].SF_overturning_E;

		VarBank.EL_Out[i].BlockId = gcnew array< Int32 >(int(Var->LevelSection[i].BlockId.size()));
		for (int j = 0; j < Var->LevelSection[i].BlockId.size(); ++j) {
			VarBank.EL_Out[i].BlockId[j] = int(Var->LevelSection[i].BlockId[j]+1);
		}
	}

	VarBank.Block_Out = gcnew array< BlockResult2 >(int(Var->BlockData.size()));
	for (int i = 0; i < Var->BlockData.size(); ++i) {
		VarBank.Block_Out[i].A = Var->BlockData[i].Area;
		VarBank.Block_Out[i].Density = Var->BlockData[i].Density;
		VarBank.Block_Out[i].Selfweight = Var->BlockData[i].SelfWeight;
		VarBank.Block_Out[i].X = Var->BlockData[i].X;
		VarBank.Block_Out[i].Mw = Var->BlockData[i].Mw;

		VarBank.Block_Out[i].EQ_Density = Var->BlockData[i].EQ_Density;
		VarBank.Block_Out[i].Selfweight_E = Var->BlockData[i].SelfWeight_E;
		VarBank.Block_Out[i].X_E = Var->BlockData[i].X_E;
		VarBank.Block_Out[i].Mw_E = Var->BlockData[i].Mw_E;
	}
	// Var Get!
	VarBank.X = Var->X;
	VarBank.X_E = Var->X_E;
	VarBank.e = Var->e;
	VarBank.e_E = Var->e_E;
	VarBank.P1 = Var->P1;
	VarBank.P2 = Var->P2;
	VarBank.P1_E = Var->P1_E;
	VarBank.P2_E = Var->P2_E;
	VarBank.bplum = Var->bplum;
	VarBank.bplum_E = Var->bplum_E;
	VarBank.sita = Var->sita;
	VarBank.sita_E = Var->sita_E;
	VarBank.b_2plum = Var->b_2plum;
	VarBank.b_2plum_E = Var->b_2plum_E;
	VarBank.R1 = Var->R1;
	VarBank.R2 = Var->R2;
	VarBank.R1_E = Var->R1_E;
	VarBank.R2_E = Var->R2_E;
	VarBank.Qu = Var->Qu;
	VarBank.Qu_E = Var->Qu_E;
	VarBank.qa = Var->qa;
	VarBank.qa_E = Var->qa_E;

	return true;
}

bool VE_SD::Module2::Run()
{
	Internal->GeoPreCal();
	Internal->WeightCal();
	Internal->EarthQuakeForceCal();
	Internal->HorizontalSoilForceCal();
	Internal->VertivalSoilForceCal();
	Internal->ResidualWaterForceCal();
	Internal->ShipTractionForceCal();
	Internal->VerticalForceSum();
	Internal->HorizontalForceSum();
	Internal->SafetyFactorCheck();
	Internal->BaseForceCheck();
	return true;
}

bool VE_SD::Module2::OutPutLogFile(String ^ Pois)
{
	// File Open
	std::ofstream FILE;
	std::string C_str = msclr::interop::marshal_as<std::string>(Pois);
	FILE.open(C_str);

	// File Contain
	FILE << "******背景參數******" << std::endl;
	FILE << "HWL: " << Var->HWL << std::endl;
	FILE << "LWL: " << Var->LWL << std::endl;
	FILE << "Ka: " << Var->ka << std::endl;

	FILE << std::endl;
	FILE << "******型塊參數******" << std::endl;
	for (size_t i = 0; i < Var->BlockData.size(); i++)
	{
		//Var->BlockData[i].Cal_Area();
		
		FILE << "區塊單元 " << i + 1 << " :" << std::endl;
		FILE << "區塊密度: " << Var->BlockData[i].Density << std::endl;
		FILE << "區塊摩擦: " << Var->BlockData[i].FrictionC << std::endl;
		FILE << "方塊面積: " << Var->BlockData[i].Area << std::endl;
		FILE << "----節點座標----" << std::endl;
		for (auto & NodeElement : Var->BlockData[i].Node) {
			FILE << "X: " << NodeElement.x << "\t" << "Y: " << NodeElement.y << std::endl;
		}
		FILE << std::endl;
		//Var->BlockData[i].SelfWeight = Var->BlockData[i].Area*Var->BlockData[i].Density;
		FILE << "----方塊自重與力矩----" << std::endl;
		FILE << "SelfWeight:" << Var->BlockData[i].SelfWeight << std::endl;
		FILE << "X:" << Var->BlockData[i].X << std::endl;
		FILE << "Mw:" << Var->BlockData[i].Mw << std::endl;
		FILE << "FrictionC:" << Var->BlockData[i].FrictionC << std::endl;
		FILE << std::endl;
	}

	FILE << std::endl;
	FILE << "******Level參數******" << std::endl;
	for (size_t i = 0; i < Var->LevelSection.size(); i++)
	{
		FILE << "EL(" << Var->LevelSection[i].Level << ")" << i + 1 << " :";
		for (size_t j = 0; j < Var->LevelSection[i].BlockId.size(); j++)
		{
			FILE << Var->LevelSection[i].BlockId[j]+1 << " ";
		}
		FILE << std::endl;
	}
	//FILE << "******Level參數-後總計******" << std::endl;
	//for (size_t i = 0; i < Var->LevelSection.size(); i++)
	//{
	//	FILE << "EL" << i + 1 << " :" << std::endl;
	//	FILE << "壁體自重-計: " << Var->LevelSection[i].Level_sum_W << std::endl;
	//	FILE << "力臂-計: " << Var->LevelSection[i].Level_total_arm << std::endl;
	//	FILE << "抵抗彎矩-計: " << Var->LevelSection[i].Level_sum_Mx << std::endl;
	//}

	//FILE << "******Level參數-前總計******" << std::endl;
	//for (size_t i = 1; i < Var->LevelSection.size(); i++)
	//{
	//	FILE << "EL" << i + 1 << " :" << std::endl;
	//	FILE << "壁體自重-計: " << Var->LevelSection[i].pre_sum_W << std::endl;
	//	FILE << "力臂-計: " << Var->LevelSection[i].pre_total_arm << std::endl;
	//	FILE << "抵抗彎矩-計: " << Var->LevelSection[i].pre_sum_Mx << std::endl;
	//}

	FILE << "******Earthquake Level參數-後總計******" << std::endl;
	for (size_t i = 0; i < Var->LevelSection.size(); i++)
	{
		FILE << "EL" << i + 1 << " :" << std::endl;
		FILE << "壁體自重-計: " << Var->LevelSection[i].Level_sum_WE << std::endl;
		FILE << "力臂-計: " << Var->LevelSection[i].Level_total_armE << std::endl;
		FILE << "抵抗彎矩-計: " << Var->LevelSection[i].Level_sum_MxE << std::endl;
	}

	FILE << "******Earthquake Level參數-前總計******" << std::endl;
	for (size_t i = 1; i < Var->LevelSection.size(); i++)
	{
		FILE << "EL" << i + 1 << " :" << std::endl;
		FILE << "壁體自重-計: " << Var->LevelSection[i].pre_sum_WE << std::endl;
		FILE << "力臂-計: " << Var->LevelSection[i].pre_total_armE << std::endl;
		FILE << "抵抗彎矩-計: " << Var->LevelSection[i].pre_sum_MxE << std::endl;
	}

	////- 表格四
	FILE << std::endl << "表格四-1" << std::endl;
	for (size_t i = 0; i < Var->LevelSection.size(); i++) {
		FILE << "EL" << i + 1 << " :" << std::endl;
		FILE << "水平分力: " << Var->LevelSection[i].Fh << std::endl;
		FILE << "力矩: " << Var->LevelSection[i].Fh_y << std::endl;
		FILE << "傾倒彎矩: " << Var->LevelSection[i].Fh_Mh << std::endl;
	}

	FILE << std::endl << "表格四-2" << std::endl;
	for (size_t i = 0; i < Var->LevelSection.size(); i++) {
		FILE << "EL" << i + 1 << " :" << std::endl;
		FILE << "水平分力: " << Var->LevelSection[i].Fh_E << std::endl;
		FILE << "力矩: " << Var->LevelSection[i].Fh_y_E << std::endl;
		FILE << "傾倒彎矩: " << Var->LevelSection[i].Fh_Mh_E << std::endl;
	}

	//FILE << std::endl << "表格四-2---後總計" << std::endl;
	//for (size_t i = 0; i < Var->LevelSection.size(); i++) {
	//	FILE << "EL" << i + 1 << " :" << std::endl;
	//	FILE << "水平分力: " << Var->LevelSection[i].Level_sum_Fh_E << std::endl;
	//	FILE << "力矩: " << Var->LevelSection[i].Level_total_Fhy_E << std::endl;
	//	FILE << "傾倒彎矩: " << Var->LevelSection[i].Level_sum_FhMh_E << std::endl;
	//}

	//FILE << std::endl << "表格四-2---前總計" << std::endl;
	//for (size_t i = 1; i < Var->LevelSection.size(); i++) {
	//	FILE << "EL" << i + 1 << " :" << std::endl;
	//	FILE << "水平分力: " << Var->LevelSection[i].pre_sum_Fh_E << std::endl;
	//	FILE << "力矩: " << Var->LevelSection[i].pre_total_Fhy_E << std::endl;
	//	FILE << "傾倒彎矩: " << Var->LevelSection[i].pre_sum_FhMh_E << std::endl;
	//}
	
	//- 表格五
	FILE << std::endl << "表格五" << std::endl;
	for (size_t i = 0; i < Var->LevelSection.size(); i++) {
		FILE << "EL" << i + 1 << " :" << std::endl;
		FILE << "垂直分力: " << Var->LevelSection[i].Level_sum_Fv << std::endl;
		FILE << "力矩: " << Var->LevelSection[i].Level_total_Fvx << std::endl;
		FILE << "抵抗彎矩: " << Var->LevelSection[i].Level_sum_FvMv << std::endl;
	}

	////- 表格六
	//FILE << std::endl << "表格六-後總計" << std::endl;
	//for (size_t i = 0; i < Var->LevelSection.size(); i++) {
	//	if (Var->LevelSection[i].Level_sum_Fw > 10e-5)
	//	{
	//		FILE << "EL" << i + 1 << " :" << std::endl;
	//		FILE << "殘留水壓: " << Var->LevelSection[i].Level_sum_Fw << std::endl;
	//		FILE << "力矩: " << Var->LevelSection[i].Level_total_Fwy << std::endl;
	//		FILE << "傾倒彎矩: " << Var->LevelSection[i].Level_sum_FwMw << std::endl;
	//	}
	//}

	//FILE << std::endl << "表格六-前總計" << std::endl;
	//for (size_t i = 1; i < Var->LevelSection.size(); i++) {
	//	if (Var->LevelSection[i].pre_sum_Fw > 10e-5)
	//	{
	//		FILE << "EL" << i + 1 << " :" << std::endl;
	//		FILE << "殘留水壓: " << Var->LevelSection[i].pre_sum_Fw << std::endl;
	//		FILE << "力矩: " << Var->LevelSection[i].pre_total_Fwy << std::endl;
	//		FILE << "傾倒彎矩: " << Var->LevelSection[i].pre_sum_FwMw << std::endl;
	//	}
	//}

	////- 表格七
	//FILE << std::endl << "表格七" << std::endl;
	//for (size_t i = 0; i < Var->LevelSection.size(); i++) {
	//	FILE << "EL" << i + 1 << " :" << std::endl;
	//	FILE << "船舶牽引力: " << Var->Ta << std::endl;
	//	FILE << "力矩: " << Var->LevelSection[i].Ft_y << std::endl;
	//	FILE << "傾倒彎矩: " << Var->LevelSection[i].Ft_Mt << std::endl;
	//}
	
	//- 表格八
	FILE << std::endl << "表格八" << std::endl;
	for (size_t i = 0; i < Var->LevelSection.size(); i++) {
		FILE << "EL" << i + 1 << " :" << std::endl;
		FILE << "平時垂直力總計: " << Var->LevelSection[i].VForcesum << std::endl;
		FILE << "平時抵抗彎矩總計: " << Var->LevelSection[i].VMomentsum << std::endl;
		FILE << "地震時垂直力總計: " << Var->LevelSection[i].VForcesum_E << std::endl;
		FILE << "地震時抵抗彎矩總計: " << Var->LevelSection[i].VMomentsum_E << std::endl;
	}

	//- 表格九
	FILE << std::endl << "表格九" << std::endl;
	for (size_t i = 0; i < Var->LevelSection.size(); i++) {
		FILE << "EL" << i + 1 << " :" << std::endl;
		FILE << "平時水平力總計: " << Var->LevelSection[i].HForcesum << std::endl;
		FILE << "平時傾倒彎矩總計: " << Var->LevelSection[i].HMomentsum << std::endl;
		FILE << "地震時水平力總計: " << Var->LevelSection[i].HForcesum_E << std::endl;
		FILE << "地震時傾倒彎矩總計: " << Var->LevelSection[i].HMomentsum_E << std::endl;
	}

	//- 表格十
	FILE << std::endl << "表格十" << std::endl;
	for (size_t i = 0; i < Var->LevelSection.size(); i++) {
		FILE << "EL" << i + 1 << " :" << std::endl;
		FILE << "平時滑動安全係數: " << Var->LevelSection[i].SF_slide << std::endl;
		FILE << "地震時滑動安全係數: " << Var->LevelSection[i].SF_slide_E << std::endl;
		FILE << "平時傾倒安全係數: " << Var->LevelSection[i].SF_overturning << std::endl;
		FILE << "地震時傾倒安全係數: " << Var->LevelSection[i].SF_overturning_E << std::endl;
	}

	FILE << std::endl << "表格底部檢核" << std::endl;
	FILE << "X " << Var->X << std::endl;
	FILE << "X E " << Var->X_E << std::endl;
	FILE << "Mr_sum " << Var->Mr_sum << std::endl;
	FILE << "Mo_sum " << Var->Mo_sum << std::endl;
	FILE << "R1 " << Var->R1 << std::endl;
	FILE << "R2 " << Var->R2 << std::endl;
	FILE << "R1 E " << Var->R1_E << std::endl;
	FILE << "R2 E " << Var->R2_E << std::endl;
	FILE << "sita " << Var->sita << std::endl;
	FILE << "sita_E " << Var->sita_E << std::endl;
	// File Close
	FILE.close();
	return true;
}
