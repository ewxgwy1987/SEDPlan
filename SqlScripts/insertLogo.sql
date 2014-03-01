GO
USE [SEDPLAN];
GO

insert into PICTURES
select 'PTERIS','Pteris Global Ltd','Logo Used On all reports for SEDPlan',
BulkColumn FROM OPENROWSET(Bulk 'F:\1Pteris Global Ltd\EWXGWY1987_DEV\SEDPlan\pteris logo (low res).jpg', SINGLE_BLOB) AS BLOB;

select * from PICTURES;
delete from PICTURES;
SELECT PIC_TITLE, PIC_IMAGE FROM PICTURES WHERE PIC_NAME = 'Charlotte-ReportLogo'