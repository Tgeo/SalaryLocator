-- living wage [table]

CREATE TABLE living_wage (
	fips_code int,
	
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
	
	primary key (fips_code)
);

insert into living_wage
select
	fips_code,
	one_adult,
	one_Adult_one_Child,
	one_Adult_two_Children,
	one_Adult_three_Children,
	two_Adults_one_working,
	two_Adults_one_working_one_Child,
	two_Adults_one_working_two_Children,
	two_Adults_one_working_three_Children,
	two_Adults,
	two_Adults_one_Child,
	two_Adults_two_Children,
	two_Adults_three_Children
from living_wage_import;
