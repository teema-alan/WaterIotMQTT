using System;
using System.Collections.Generic;

namespace WaterIot.Models
{
	/// <summary>
	/// 監測項目分類 Category for Physical Quantity
	/// </summary>
	public class CategoryInfo : InfoBase
	{
		/// <summary>
		/// [必要] 分類 Id
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// [必要] 名稱
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// [非必要] 描述
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// [非必要]
		/// </summary>
		public IEnumerable<PhysicalQuantityInfo> PhysicalQuantities { get; set; }
	}
}