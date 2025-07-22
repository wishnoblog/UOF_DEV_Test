SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Z_FCF_SAP_REQ](
	[DOC_NBR] [varchar](50) NOT NULL,
	[APPLICANT] [nvarchar](50) NULL,
	[APPLICANT_NAME] [nvarchar](50) NULL,
	[APPLICANTDEPT] [nvarchar](50) NULL,
	[APPLICANTDEPT_NAME] [nvarchar](50) NULL,
	[SUBJECT] [nvarchar](50) NULL,
	[CONTENT] [nvarchar](50) NULL,
	[CATALOG1] [nvarchar](50) NULL,
	[CATALOG2] [nvarchar](50) NULL,
	[CCRM] [nvarchar](50) NULL,
	[SAP_RESPONCE] [nvarchar](50) NULL
) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申請者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Z_FCF_SAP_REQ', @level2type=N'COLUMN',@level2name=N'APPLICANT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申請者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Z_FCF_SAP_REQ', @level2type=N'COLUMN',@level2name=N'APPLICANT_NAME'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申請部門' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Z_FCF_SAP_REQ', @level2type=N'COLUMN',@level2name=N'APPLICANTDEPT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申請者部門名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Z_FCF_SAP_REQ', @level2type=N'COLUMN',@level2name=N'APPLICANTDEPT_NAME'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主旨' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Z_FCF_SAP_REQ', @level2type=N'COLUMN',@level2name=N'SUBJECT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'說明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Z_FCF_SAP_REQ', @level2type=N'COLUMN',@level2name=N'CONTENT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分類1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Z_FCF_SAP_REQ', @level2type=N'COLUMN',@level2name=N'CATALOG1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分類2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Z_FCF_SAP_REQ', @level2type=N'COLUMN',@level2name=N'CATALOG2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CCRM備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Z_FCF_SAP_REQ', @level2type=N'COLUMN',@level2name=N'CCRM'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SAP處理情形' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Z_FCF_SAP_REQ', @level2type=N'COLUMN',@level2name=N'SAP_RESPONCE'
GOSET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Z_FCF_SAP_REQ](
	[DOC_NBR] [varchar](50) NOT NULL,
	[APPLICANT] [nvarchar](50) NULL,
	[APPLICANT_NAME] [nvarchar](50) NULL,
	[APPLICANTDEPT] [nvarchar](50) NULL,
	[APPLICANTDEPT_NAME] [nvarchar](50) NULL,
	[SUBJECT] [nvarchar](50) NULL,
	[CONTENT] [nvarchar](50) NULL,
	[CATALOG1] [nvarchar](50) NULL,
	[CATALOG2] [nvarchar](50) NULL,
	[CCRM] [nvarchar](50) NULL,
	[SAP_RESPONCE] [nvarchar](50) NULL
) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申請者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Z_FCF_SAP_REQ', @level2type=N'COLUMN',@level2name=N'APPLICANT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申請者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Z_FCF_SAP_REQ', @level2type=N'COLUMN',@level2name=N'APPLICANT_NAME'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申請部門' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Z_FCF_SAP_REQ', @level2type=N'COLUMN',@level2name=N'APPLICANTDEPT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申請者部門名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Z_FCF_SAP_REQ', @level2type=N'COLUMN',@level2name=N'APPLICANTDEPT_NAME'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主旨' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Z_FCF_SAP_REQ', @level2type=N'COLUMN',@level2name=N'SUBJECT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'說明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Z_FCF_SAP_REQ', @level2type=N'COLUMN',@level2name=N'CONTENT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分類1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Z_FCF_SAP_REQ', @level2type=N'COLUMN',@level2name=N'CATALOG1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分類2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Z_FCF_SAP_REQ', @level2type=N'COLUMN',@level2name=N'CATALOG2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CCRM備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Z_FCF_SAP_REQ', @level2type=N'COLUMN',@level2name=N'CCRM'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SAP處理情形' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Z_FCF_SAP_REQ', @level2type=N'COLUMN',@level2name=N'SAP_RESPONCE'
GO