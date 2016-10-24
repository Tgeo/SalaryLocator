-- Fixes a few (that I could find) NECTA designated areas in the living wage data.

update living_wage_import
set fips_code = 70900
where fips_code = 12700;

update living_wage_import
set fips_code = 71650
where fips_code = 14460;

update living_wage_import
set fips_code = 76600
where fips_code = 38340;

update living_wage_import
set fips_code = 78100
where fips_code = 44140;

update living_wage_import
set fips_code = 79600
where fips_code = 49340;
