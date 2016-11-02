namespace MagicCircuit
{
	public static class Constant
	{
		// Utils.cs : CamQuad original point(x, y) for coverting cordinate from frameImg to Unity3D
		public const int    CAM_QUAD_ORIGINAL_POINT_X = -301;
		public const int    CAM_QUAD_ORIGINAL_POINT_Y = 372;

		// CardDetector.cs : Parameters for limiting detected cards
		public const int    CARD_MIN_SQUARE_LEN       = 20;
		public const int    CARD_MAX_SQUARE_LEN       = 55;
		public const double CARD_MAX_SQUARE_LEN_RATIO = 1.5;
		public const double CARD_OUTER_SQUARE_RATIO   = 1.2;



	}
}