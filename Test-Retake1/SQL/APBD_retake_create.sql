-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2022-06-21 06:44:31.072

-- tables
-- Table: Album
CREATE TABLE Album (
    IdAlbum int  NOT NULL,
    AlbumName nvarchar(50)  NOT NULL,
    PublishDate datetime  NOT NULL,
    IdMusicLabel int  NOT NULL,
    CONSTRAINT Album_pk PRIMARY KEY  (IdAlbum)
);

-- Table: MusicLabel
CREATE TABLE MusicLabel (
    IdMusicLabel int  NOT NULL,
    Name nvarchar(50)  NOT NULL,
    CONSTRAINT MusicLabel_pk PRIMARY KEY  (IdMusicLabel)
);

-- Table: Musician
CREATE TABLE Musician (
    IdMusician int  NOT NULL,
    FirstName nvarchar(30)  NOT NULL,
    LastName nvarchar(50)  NOT NULL,
    Nickname nvarchar(20)  NULL,
    CONSTRAINT Musician_pk PRIMARY KEY  (IdMusician)
);

-- Table: Musician_Track
CREATE TABLE Musician_Track (
    IdMusician int  NOT NULL,
    IdTrack int  NOT NULL,
    CONSTRAINT Musician_Track_pk PRIMARY KEY  (IdMusician,IdTrack)
);

-- Table: Track
CREATE TABLE Track (
    IdTrack int  NOT NULL,
    TrackName nvarchar(50)  NOT NULL,
    Duration float(5)  NOT NULL,
    IdAlbum int  NULL,
    CONSTRAINT Track_pk PRIMARY KEY  (IdTrack)
);

-- foreign keys
-- Reference: Album_MusicLabel (table: Album)
ALTER TABLE Album ADD CONSTRAINT Album_MusicLabel
    FOREIGN KEY (IdMusicLabel)
    REFERENCES MusicLabel (IdMusicLabel);

-- Reference: Musician_Track_Musician (table: Musician_Track)
ALTER TABLE Musician_Track ADD CONSTRAINT Musician_Track_Musician
    FOREIGN KEY (IdMusician)
    REFERENCES Musician (IdMusician);

-- Reference: Musician_Track_Track (table: Musician_Track)
ALTER TABLE Musician_Track ADD CONSTRAINT Musician_Track_Track
    FOREIGN KEY (IdTrack)
    REFERENCES Track (IdTrack);

-- Reference: Track_Album (table: Track)
ALTER TABLE Track ADD CONSTRAINT Track_Album
    FOREIGN KEY (IdAlbum)
    REFERENCES Album (IdAlbum);

-- End of file.

