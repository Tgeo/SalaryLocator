
CREATE TABLE occupation (
	occupation_code char(7) PRIMARY KEY,
	occupation_group int not null,
	title text not null
	--is_annual_salary boolean not null -- If false, then it is hourly
);

-- occupation_group values:
-- 1 = total
-- 2 = major
-- 3 = detailed

insert into occupation
select occ_code as occupation_code, 3 as occupation_group, occ_title as title
from salaries
where occ_group = 'detailed'
group by (occ_code, occ_title, occ_group);

insert into occupation
select occ_code as occupation_code, 2 as occupation_group, occ_title as title
from salaries
where occ_group = 'major'
group by (occ_code, occ_title, occ_group);

insert into occupation
select occ_code as occupation_code, 1 as occupation_group, occ_title as title
from salaries
where occ_group = 'total'
group by (occ_code, occ_title, occ_group);
