namespace MagicCircuit
{
	public static class Constant
	{
		// GetImage.cs : Parameter for processing 10 photos
		public const int    THREAD_TAKE_NUM_OF_PHOTOS = 2;

		// Utils.cs : CamQuad original point(x, y) for coverting cordinate from frameImg to Unity3D
		public const int    CAM_QUAD_ORIGINAL_POINT_X = -301;
		public const int    CAM_QUAD_ORIGINAL_POINT_Y = 372;

		// RotateCamera.cs : Width and Height of CamQuad in Unity3D
		public const int    CAM_QUAD_WIDTH            = 603;
		public const int    CAM_QUAD_HEIGHT           = 698;

		// CardDetector.cs : Parameters for limiting and filtering detected cards
		public const int    CARD_MIN_SQUARE_LEN       = 60;
		public const int    CARD_MAX_SQUARE_LEN       = 100;
		public const double CARD_MAX_SQUARE_LEN_RATIO = 1.3;
		public const double CARD_OUTER_SQUARE_RATIO   = 1.2;

		// LineDetector.cs : Parameter for limiting and filtering detected lines
		public const int    LINE_REGION_MIN_AREA      = 0;
	}
}