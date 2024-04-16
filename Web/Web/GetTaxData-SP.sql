USE [TaxSystem]
GO

/****** Object:  StoredProcedure [dbo].[GetTaxData]    Script Date: 02/03/24 21:30:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE Procedure [dbo].[GetTaxData]
(
		@TaxNumber char(22),	
		@InvoiceSubject	tinyint,			--   فاکتور اصلی =1   فاکتور اصلاحی =2   فاکتور ابطالی=3    برگشت از فروش=4
		@OwnerCompany	int     
)
--بابت برگشت وابطال و اصلاح باید بررسی و اصلاح شود
--فرض اولیه بدون تخفیف بودن فاکتور است باید اصلاح شود

-- فروش
as
begin

SELECT			/*1*/InvoiceData.InvoiceTaxNumber,
				/*2*/InvoiceData.UnixInvoiceDate,
				/*3*/InvoiceData.UnixCreationDate,
				/*4*/InvoiceData.TaxInvoiceType,
				/*5*/InvoiceData.InvoiceTaxSerialNumber,
				/*6*/null as ReferenceInvoiceTaxNumber,
				/*7*/InvoiceData.InvoiceTaxPattern,
				/*8*/@InvoiceSubject AS InvoiceSubject,
                /*9*/ InvoiceData.EconomicCodeSeller, 
				/*10*/InvoiceData.EntityType, 
				/*11*/-- 
				/*12*/InvoiceData.NationalCode, 
				
				/*13*/--
				/*14*/--
				/*15*/--
				/*16*/--
				/*17*/--
				/*18*/--
				/*19*/--
				/*20*/--
				/*21*/--
				/*22*/--
				/*23*/--
				/*24*/ sum(InvoiceData.GoodsPriceBeforDiscount) as SumPriceBeforDiscount,
				/*25*/sum(InvoiceData.GoodsPriceDiscount) as SumPriceDiscount,
				/*26*/sum(InvoiceData.GoodsPriceAfterDiscount) as SumPriceAfterDiscount,
				/*27*/sum(InvoiceData.VATPrice) as SumVatPrice,
				/*28*/ 0 as SumOtherTaxPrice,
				/*29*/sum(GoodsTotalPrice) as SumTotalPrice,
				/*30*/--
				/*31*/--
				/*32*/--
				/*33*/ InvoiceData.InvoiceSettlementType,
				/*34*/--
				/*35*/--
				/*36*/--
				/*37*/--
				/*38*/InvoiceData.DisplayCode,
				/*39*/InvoiceData.GoodsNameInvoice,
				/*40*/InvoiceData.Quantity,
				/*41*/--
				/*42*/--
				/*43*/InvoiceData.UnitPrice,
				/*44*/--
				/*45*/--
				/*46*/--
				/*47*/--
				/*48*/--
				/*49*/  InvoiceData.GoodsPriceBeforDiscount, --باید رند شود
				/*50*/  InvoiceData.GoodsPriceDiscount,
				/*51*/  InvoiceData.GoodsPriceAfterDiscount,-- رند شود
				/*52*/ InvoiceData.VATRate,
				/*53*/InvoiceData.VATPrice,
				/*54*/--
				/*55*/--
				/*56*/--
				/*57*/--
				/*58*/--
				/*59*/--
				/*60*/InvoiceData.GoldProductionCost,
				/*61*/InvoiceData.GoldBenefit,
				/*62*/InvoiceData.Goldhagh,
				/*63*/InvoiceData.GoldProductionCost + InvoiceData.GoldBenefit + InvoiceData.Goldhagh as SumGold,
				/*64*/-- 
				/*65*/--
				/*66*/--
				/*67*/InvoiceData.GoodsTotalPrice
				/*68*/--
				/*69*/--
				/*70*/--
				/*71*/--
				/*72*/--
				/*73*/--
				/*74*/--
				/*75*/--
				/*76*/--



				FROM (
				select			
								Invoice.InvoiceTaxNumber,
								Invoice.UnixInvoiceDate,
								Invoice.UnixCreationDate,
								Invoice.InvoiceTaxSerialNumber,
								Invoice.InvoiceSettlementType,
								Invoice.TaxInvoiceType,
								Invoice.InvoiceTaxPattern,
							 	Invoice.EconomicCodeSeller,
								Invoice.EntityType,
								Invoice.NationalCode,
								InvoiceGoods.DisplayCode,
								InvoiceGoods.GoodsSmallName,
								InvoiceGoods.Quantity,
								InvoiceGoods.Description as GoodsNameInvoice,
								InvoiceGoods.UnitPrice,
								InvoiceGoods.Quantity*InvoiceGoods.UnitPrice as GoodsPriceBeforDiscount,   --باید رند شود
								InvoiceGoods.GoodsPriceDiscount,
								(InvoiceGoods.Quantity*InvoiceGoods.UnitPrice) - isnull(InvoiceGoods.GoodsPriceDiscount ,0) GoodsPriceAfterDiscount,  -- رند شود
								InvoiceGoods.VATRate, 
								InvoiceGoods.VATPrice,
								0 as GoldProductionCost,
								0 as GoldBenefit,
								0 as Goldhagh,
								(InvoiceGoods.Quantity*InvoiceGoods.UnitPrice) - isnull(InvoiceGoods.GoodsPriceDiscount ,0) + InvoiceGoods.VATPrice as GoodsTotalPrice

								from   dbo.Invoices Invoice inner join dbo.InvoiceItems InvoiceGoods on Invoice.ID =INvoiceGoods.InvoiceId
								 ) as InvoiceData 

where InvoiceData.InvoiceTaxNumber=@TaxNumber
--and @InvoiceSubject=1   -- فاکتور اصلی

GROUP BY InvoiceData.InvoiceTaxNumber, InvoiceData.UnixInvoiceDate, InvoiceData.UnixCreationDate, InvoiceData.TaxInvoiceType, InvoiceData.InvoiceTaxSerialNumber,  InvoiceData.InvoiceTaxPattern, 
                     InvoiceData.EntityType, InvoiceData.NationalCode,InvoiceData.EconomicCodeSeller,  InvoiceData.InvoiceSettlementType, InvoiceData.DisplayCode,InvoiceData.GoodsSmallName, InvoiceData.Quantity, InvoiceData.UnitPrice, InvoiceData.GoodsPriceBeforDiscount, 
                  InvoiceData.GoodsPriceDiscount, InvoiceData.GoodsPriceAfterDiscount, InvoiceData.VATRate, InvoiceData.VATPrice, InvoiceData.GoldProductionCost, InvoiceData.GoldBenefit, InvoiceData.Goldhagh, 
                  InvoiceData.GoldProductionCost + InvoiceData.GoldBenefit + InvoiceData.Goldhagh, InvoiceData.GoodsTotalPrice,InvoiceData.GoodsNameInvoice

end

 

GO


