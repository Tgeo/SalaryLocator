
--drop table living_wage_import;

CREATE TABLE living_wage_import(
  one_adult money,
  one_Adult_one_Child money,
  one_Adult_two_Children money,
  one_Adult_three_Children money,
  two_Adults_one_working money,
  two_Adults_one_working_one_Child money,
  two_Adults_one_working_two_Children money,
  two_Adults_one_working_three_Children money,
  two_Adults money,
  two_Adults_one_Child money,
  two_Adults_two_Children money,
  two_Adults_three_Children money,
  FIPS_Code int,
  Location text,
  year int
);

-- .CSV file generated via 'Save-As' from Excel.
COPY living_wage_import
FROM 'C:\dev\salarylocator\data\living_wage_data\2015_Living_Wage.csv' DELIMITER ',' CSV HEADER;

--select * from living_wage_import;


--select area.* from living_wage_import imp
--right outer join area on area_code = fips_code
--where fips_code is null