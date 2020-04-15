--Use npcampground;
DELETE FROM reservation;
DELETE FROM site;
DELETE FROM campground;
DELETE FROM park;



-- Insert a fake Park
INSERT INTO park
(name, location, establish_date, area, visitors, description)
values('Tranquility Base', 'The Moon',  '1969-06-15', 2500, 12 ,'Site of manned mission to our only natural satellite');
DECLARE @park_id int = (SELECT @@IDENTITY);

-- Insert fake Campground
INSERT INTO campground
(park_id, name, open_from_mm, open_to_mm, daily_fee) 
values(@park_id,'Moonbase Alpha',1 , 12, 5000.00 )
DECLARE @campground_id int = (SELECT @@IDENTITY);


--Insert fake site
INSERT INTO site
(campground_id, site_number, max_occupancy, accessible, max_rv_length, utilities)
values(@campground_id, 100, 12, 0, 0, 0),
(@campground_id, 200, 6, 1, 20, 1)
DECLARE @site_id int = (SELECT @@IDENTITY);

-- Insert fake reservation
INSERT INTO reservation
(site_id, name, from_date, to_date, create_date)
values (@site_id, 'Asimov Family', '2020-06-15','2020-06-30', '2020-05-01') ;
DECLARE @reservation_id int = (SELECT @@IDENTITY)
SELECT @park_id as park_id, @reservation_id as reservation_id, @campground_id as campground_id, @site_id as site_id
