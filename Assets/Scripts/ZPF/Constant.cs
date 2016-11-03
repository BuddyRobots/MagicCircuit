namespace MagicCircuit
{
	public static class Constant
	{
		// GetImage.cs : Parameter for processing 10 photos
		public const int    TAKE_NUM_OF_PHOTOS = 2;

		// Utils.cs : CamQuad original point(x, y) for coverting cordinate from frameImg to Unity3D
		public const int    CAM_QUAD_ORIGINAL_POINT_X = -305;
		public const int    CAM_QUAD_ORIGINAL_POINT_Y = 375;

		// RotateCamera.cs : Width and Height of CamQuad in Unity3D
		public const int    CAM_QUAD_WIDTH            = 610;
		public const int    CAM_QUAD_HEIGHT           = 703;

		// CardDetector.cs : Parameters for limiting and filtering detected cards
		public const int    CARD_MIN_SQUARE_LEN       = 70;
		public const int    CARD_MAX_SQUARE_LEN       = 100;
		public const double CARD_MAX_SQUARE_LEN_RATIO = 1.3;
		public const double CARD_OUTER_SQUARE_RATIO   = 1.3;

		// LineDetector.cs : Parameter for limiting and filtering detected lines
		public const int    LINE_COLOR_MAX_V          = 170;
		public const int    LINE_REGION_MIN_AREA      = 0;


		//PhotoRecognizingPanel: arrow show interval
		public const float  ARROW_GEN_INTERVAL	=	0.8f;

		// CurrentFlow.cs : Parameter for determining whether to points are connected
		public const int    POINT_CONNECT_REGION      = 40;

	}
}