using System;

namespace WaterIot.Models
{
	/// <summary>
	/// 所有物件設定參數之共同參數
	/// </summary>
	public class InfoBase
	{
		/// <summary>
		/// [必要] 建立日期 get or set Create Date
		/// </summary>
		public DateTimeOffset CreateDate { get; set; }

		/// <summary>
		/// [必要] 最後一次修改日期 get or set Modify Date
		/// </summary>
		public DateTimeOffset ModifyDate { get; set; }

		/// <summary>
		/// [必要] FK[Loose Couple with UserInfo.Account , no cascade delete] 使用者帳戶名稱，非Id
		/// </summary>
		public string Creator { get; set; }

		/// <summary>
		/// [必要] 修改者 get or set Modifier
		/// </summary>
		public string Modifier { get; set; }

		/// <summary>
		/// [必要] 排序 get or set SortIndex
		/// </summary>
		public int SortIndex { get; set; }
	}
}