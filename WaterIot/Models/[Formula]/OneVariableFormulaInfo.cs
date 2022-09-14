using System;

namespace WaterIot.Models
{
	/// <summary>
	/// 單變數轉換公式，於收監測資料時會立即套用運算的公式
	/// </summary>
	public class OneVariableFormulaInfo : InfoBase
	{
		/// <summary>
		/// [必要] 單一變數轉換公式Id
		/// </summary>
		/// <value>The identifier.</value>
		public Guid Id { get; set; }

		/// <summary>
		/// [非必要] 公式描述
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// [必要] Senslink 3.0 新增，自定義單變數公式
		/// </summary>
		/// <value>The custom formula expression.</value>
		public string FormulaExpression { get; set; }

		/// <summary>
		/// [必要] FK: PhysicalQuantityInfo.Id
		/// </summary>
		/// <value>The physical quantity_ identifier.</value>
		public Guid PhysicalQuantity_Id { get; set; }
	}
}