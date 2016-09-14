
CREATE TABLE salary (
	area_code int,
	occupation_code char(7),
	
	total_employment int,
	employment_std_err decimal,
	jobs_per_1000 decimal,
	location_quotient decimal,
	
	hourly_mean decimal,
	annual_mean int,
	mean_std_err decimal,
	
	hourly_10_pct decimal,
	hourly_25_pct decimal,
	hourly_med_pct decimal,
	hourly_75_pct decimal,
	hourly_90_pct decimal,
	
	annual_10_pct int,
	annual_25_pct int,
	annual_med_pct int,
	annual_75_pct int,
	annual_90_pct int,
	
	primary key (area_code, occupation_code)
);

insert into salary
select
	CAST(area as int) as area_code,
	occ_code as occupation_code,
	
	tot_emp,
	emp_prse,
	jobs_1000,
	loc_quotient,
	h_mean,
	a_mean,
	mean_prse,
	h_pct10,
	h_pct25,
	h_median,
	h_pct75,
	h_pct90,
	a_pct10,
	a_pct25,
	a_median,
	a_pct75,
	a_pct90
from salaries