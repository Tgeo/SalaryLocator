
CREATE TEMP TABLE msa_to_division_temp (
	msa_code int,
	msa_division int,
	
	primary key (msa_code, msa_division)
);

-- Chicago-Naperville-Elgin, Ill.-Ind.-Wis. Metropolitan Statistical Area
insert into msa_to_division_temp values (16980, 16974);
insert into msa_to_division_temp values (16980, 20994);
insert into msa_to_division_temp values (16980, 23844);
insert into msa_to_division_temp values (16980, 29404);

-- Dallas-Fort Worth-Arlington, Texas Metropolitan Statistical Area
insert into msa_to_division_temp values (19100, 19124);
insert into msa_to_division_temp values (19100, 23104);

--Detroit-Warren-Dearborn, Mich. Metropolitan Statistical Area
insert into msa_to_division_temp values (19820, 19804);
insert into msa_to_division_temp values (19820, 47664);

--Los Angeles-Long Beach-Anaheim, Calif. Metropolitan Statistical Area
insert into msa_to_division_temp values (31080, 11244);
insert into msa_to_division_temp values (31080, 31084);

--Miami-Fort Lauderdale-West Palm Beach, Fla. Metropolitan Statistical Area
insert into msa_to_division_temp values (33100, 22744);
insert into msa_to_division_temp values (33100, 33124);
insert into msa_to_division_temp values (33100, 48424);

--New York-Newark-Jersey City, N.Y.-N.J.-Pa. Metropolitan Statistical Area
insert into msa_to_division_temp values (35620, 20524);
insert into msa_to_division_temp values (35620, 35004);
insert into msa_to_division_temp values (35620, 35084);
insert into msa_to_division_temp values (35620, 35614);

--Philadelphia-Camden-Wilmington, Pa.-N.J.-Del.-Md. Metropolitan Statistical Area
insert into msa_to_division_temp values (37980, 15804);
insert into msa_to_division_temp values (37980, 33874);
insert into msa_to_division_temp values (37980, 37964);
insert into msa_to_division_temp values (37980, 48864);

--San Francisco-Oakland-Hayward, Calif. Metropolitan Statistical Area
insert into msa_to_division_temp values (41860, 36084);
insert into msa_to_division_temp values (41860, 41884);
insert into msa_to_division_temp values (41860, 42034);

--Seattle-Tacoma-Bellevue, Wash. Metropolitan Statistical Area
insert into msa_to_division_temp values (42660, 42644);
insert into msa_to_division_temp values (42660, 45104);

--Washington-Arlington-Alexandria, D.C.-Va.-Md.-W.Va. Metropolitan Statistical Area
insert into msa_to_division_temp values (47900, 43524);
insert into msa_to_division_temp values (47900, 47894);

--Boston-Cambridge-Nashua, Mass.-N.H. Metropolitan NECTA
insert into msa_to_division_temp values (71650, 71654);
insert into msa_to_division_temp values (71650, 72104);
insert into msa_to_division_temp values (71650, 73104);
insert into msa_to_division_temp values (71650, 73604);
insert into msa_to_division_temp values (71650, 74204);
insert into msa_to_division_temp values (71650, 74804);
insert into msa_to_division_temp values (71650, 74854);
insert into msa_to_division_temp values (71650, 75404);
insert into msa_to_division_temp values (71650, 76524);
insert into msa_to_division_temp values (71650, 78254);



-- Add the [MSA_CODE] column to [area] table

alter table area add column msa_code int null;

update area
set msa_code = (
	select temp.msa_code
	from msa_to_division_temp temp
	where temp.msa_division = area.area_code
	limit 1
);

-- otherwise just copy the area_code
update area
set msa_code = area_code
where msa_code is null;

alter table area alter column msa_code set not null;

