
CREATE TABLE area (
	area_code int PRIMARY KEY,
	primary_state_code char(2) references state(state_code) not null,
	name text not null
);

insert into area
select CAST(area AS integer) as area_code, prim_state as primary_state_code, area_name as name
	from salaries
	group by (prim_state, area, area_name);
