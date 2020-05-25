drop table if exists car;
create table car (
	id bigint not null unique,
	name text not null,
	year timestamp not null,

	primary key (id)
);

drop table  if exists  person;
create table person (
	id bigint not null unique,
	name text not null,
	age int not null,

	primary key(id)
);

drop table  if exists  car_n_person;
create table car_n_person (
	person_id bigint not null,
	car_id bigint not null,

	foreign key (person_id) references person(id) on delete cascade,
	foreign key (car_id) references car(id) on delete cascade
);

