using System;

namespace OSECircuitRender
{
	public sealed class Workbook
	{
		public Worksheets Sheets = new();

		public Worksheet AddNewSheet()
		{
			Worksheet ws = new();
			Log.L("Adding sheet");
			Sheets.AddSheet(ws);
			return ws;
		}
	}

	public static class Log
	{
		public static void L(string text)
		{
			Method?.Invoke(text);
		}
		public static Action<string> Method;
	}

}