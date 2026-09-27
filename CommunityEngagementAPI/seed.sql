--
-- PostgreSQL database dump
--



-- Dumped from database version 18.3
-- Dumped by pg_dump version 18.3

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;

SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

ALTER TABLE ONLY public.volunteer DROP CONSTRAINT volunteer_volunteer_id_fkey;
ALTER TABLE ONLY public.volunteer DROP CONSTRAINT volunteer_recruiter_id_fkey;
ALTER TABLE ONLY public.user_demographics DROP CONSTRAINT userdemographics_user_id_fkey;
ALTER TABLE ONLY public.volunteer_sdg DROP CONSTRAINT user_sdg_user_id_fkey;
ALTER TABLE ONLY public.volunteer_sdg DROP CONSTRAINT user_sdg_sdg_id_fkey;
ALTER TABLE ONLY public."user" DROP CONSTRAINT user___fk;
ALTER TABLE ONLY public.suspension DROP CONSTRAINT suspension___fk;
ALTER TABLE ONLY public.student DROP CONSTRAINT student_student_id_fkey;
ALTER TABLE ONLY public.staff DROP CONSTRAINT staff___fk;
ALTER TABLE ONLY public.report DROP CONSTRAINT report___fk_2;
ALTER TABLE ONLY public.report DROP CONSTRAINT report___fk;
ALTER TABLE ONLY public.qrcode DROP CONSTRAINT qrcode___fk_2;
ALTER TABLE ONLY public.qrcode DROP CONSTRAINT qrcode___fk;
ALTER TABLE ONLY public.project_register DROP CONSTRAINT project_volunteering_user_id_fkey;
ALTER TABLE ONLY public.project_register DROP CONSTRAINT project_volunteering_project_id_fkey;
ALTER TABLE ONLY public.project_summary DROP CONSTRAINT project_summary___fk;
ALTER TABLE ONLY public.project_sdg DROP CONSTRAINT project_sdg_sdg_id_fkey;
ALTER TABLE ONLY public.project_sdg DROP CONSTRAINT project_sdg_project_id_fkey;
ALTER TABLE ONLY public.project DROP CONSTRAINT project___fk_2;
ALTER TABLE ONLY public.project DROP CONSTRAINT project___fk;
ALTER TABLE ONLY public.picture DROP CONSTRAINT picture___fk;
ALTER TABLE ONLY public.notification DROP CONSTRAINT notification___fk;
ALTER TABLE ONLY public.ngo_sdg DROP CONSTRAINT ngo_sdg___fk_2;
ALTER TABLE ONLY public.ngo_sdg DROP CONSTRAINT ngo_sdg___fk;
ALTER TABLE ONLY public.ngo_partnership DROP CONSTRAINT ngo_partnership___fk;
ALTER TABLE ONLY public.ngo_application DROP CONSTRAINT ngo_application___fk;
ALTER TABLE ONLY public.department DROP CONSTRAINT department___fk;
ALTER TABLE ONLY public.certificate DROP CONSTRAINT certificate___fk;
ALTER TABLE ONLY public.attendance_register DROP CONSTRAINT attendance_register___fk_3;
ALTER TABLE ONLY public.attendance_register DROP CONSTRAINT attendance_register___fk;
ALTER TABLE ONLY public.attendance_entry DROP CONSTRAINT attendance_entry___fk_2;
ALTER TABLE ONLY public.attendance_entry DROP CONSTRAINT attendance_entry___fk;
ALTER TABLE ONLY public.alumni DROP CONSTRAINT alumni___fk;
ALTER TABLE ONLY public."user" DROP CONSTRAINT "User_campus_id_fkey";
ALTER TABLE ONLY public.volunteer DROP CONSTRAINT volunteer_pk;
ALTER TABLE ONLY public.volunteer_sdg DROP CONSTRAINT user_sdg_pkey;
ALTER TABLE ONLY public."user" DROP CONSTRAINT user_pk;
ALTER TABLE ONLY public.user_demographics DROP CONSTRAINT user_demographics_pk;
ALTER TABLE ONLY public.unregistered_volunteer DROP CONSTRAINT unregistered_volunteer_pk;
ALTER TABLE ONLY public.suspension DROP CONSTRAINT suspension_pk;
ALTER TABLE ONLY public.student DROP CONSTRAINT student_pk;
ALTER TABLE ONLY public.staff DROP CONSTRAINT staff_pk;
ALTER TABLE ONLY public.sdg DROP CONSTRAINT sdg_pk;
ALTER TABLE ONLY public.report DROP CONSTRAINT report_pk;
ALTER TABLE ONLY public.qrcode DROP CONSTRAINT qrcode_pk;
ALTER TABLE ONLY public.project_register DROP CONSTRAINT project_volunteering_pkey;
ALTER TABLE ONLY public.project_summary DROP CONSTRAINT project_summary_pk;
ALTER TABLE ONLY public.project_sdg DROP CONSTRAINT project_sdg_pkey;
ALTER TABLE ONLY public.project DROP CONSTRAINT project_id;
ALTER TABLE ONLY public."__EFMigrationsHistory" DROP CONSTRAINT pk___ef_migrations_history;
ALTER TABLE ONLY public.picture DROP CONSTRAINT picture_pk;
ALTER TABLE ONLY public.notification DROP CONSTRAINT notification_pk;
ALTER TABLE ONLY public.ngo_sdg DROP CONSTRAINT ngo_sdg_pk;
ALTER TABLE ONLY public.ngo_partnership DROP CONSTRAINT ngo_partnership_pk;
ALTER TABLE ONLY public.ngo_application DROP CONSTRAINT ngo_application_pk;
ALTER TABLE ONLY public.faculty DROP CONSTRAINT faculty_pk;
ALTER TABLE ONLY public.document DROP CONSTRAINT document_pk;
ALTER TABLE ONLY public.department DROP CONSTRAINT department_pk;
ALTER TABLE ONLY public.certificate DROP CONSTRAINT certificate_pk;
ALTER TABLE ONLY public.campus DROP CONSTRAINT campus_pk;
ALTER TABLE ONLY public.attendance_register DROP CONSTRAINT attendance_register_pk;
ALTER TABLE ONLY public.attendance_entry DROP CONSTRAINT attendance_entry_pk;
ALTER TABLE ONLY public.alumni DROP CONSTRAINT alumni_pk;
DROP TABLE public.volunteer_sdg;
DROP TABLE public.volunteer;
DROP TABLE public.user_demographics;
DROP TABLE public.unregistered_volunteer;
DROP TABLE public.suspension;
DROP TABLE public.student;
DROP TABLE public.staff;
DROP TABLE public.sponsor;
DROP TABLE public.sdg;
DROP TABLE public.report;
DROP TABLE public.qrcode;
DROP TABLE public.project_summary;
DROP TABLE public.project_sdg;
DROP TABLE public.project_register;
DROP TABLE public.project;
DROP TABLE public.picture;
DROP TABLE public.photo;
DROP TABLE public.notification;
DROP TABLE public.ngo_sdg;
DROP TABLE public.ngo_partnership;
DROP TABLE public.ngo_application;
DROP TABLE public.faculty;
DROP TABLE public.document;
DROP TABLE public.department;
DROP TABLE public.certificate;
DROP TABLE public.campus;
DROP TABLE public.attendance_entry;
DROP TABLE public.attendance_register;
DROP TABLE public.alumni;
DROP TABLE public."__EFMigrationsHistory";
DROP TABLE public."user";
DROP TYPE public.year_type;
DROP TYPE public.v_type;
DROP TYPE public.gender_type;
DROP TYPE public.code_type;
--
-- Name: code_type; Type: TYPE; Schema: public; Owner: postgres
--

CREATE TYPE public.code_type AS ENUM (
    'APK',
    'APB',
    'DFC',
    'SWC'
);


ALTER TYPE public.code_type OWNER TO postgres;

--
-- Name: gender_type; Type: TYPE; Schema: public; Owner: postgres
--

CREATE TYPE public.gender_type AS ENUM (
    'Male',
    'Female'
);


ALTER TYPE public.gender_type OWNER TO postgres;

--
-- Name: v_type; Type: TYPE; Schema: public; Owner: postgres
--

CREATE TYPE public.v_type AS ENUM (
    'Student',
    'Alumni',
    'Staff'
);


ALTER TYPE public.v_type OWNER TO postgres;

--
-- Name: year_type; Type: TYPE; Schema: public; Owner: postgres
--

CREATE TYPE public.year_type AS ENUM (
    '1st',
    '2rd',
    '3rd',
    '4th',
    'Post'
);


ALTER TYPE public.year_type OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: user; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."user" (
    user_id integer CONSTRAINT "User_user_id_not_null" NOT NULL,
    first_name character varying CONSTRAINT "User_first_name_not_null" NOT NULL,
    last_name character varying CONSTRAINT "User_last_name_not_null" NOT NULL,
    email character varying CONSTRAINT "User_email_not_null" NOT NULL,
    phone character varying CONSTRAINT "User_phone_not_null" NOT NULL,
    id_number character varying CONSTRAINT "User_id_number_not_null" NOT NULL,
    password character varying CONSTRAINT "User_password_not_null" NOT NULL,
    date_created date CONSTRAINT "User_date_created_not_null" NOT NULL,
    campus_id integer CONSTRAINT "User_campus_id_not_null" NOT NULL,
    is_active boolean NOT NULL,
    is_suspended boolean NOT NULL,
    role character varying CONSTRAINT user_role_type_not_null NOT NULL,
    faculty_id integer
);


ALTER TABLE public."user" OWNER TO postgres;

--
-- Name: User_user_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."user" ALTER COLUMN user_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."User_user_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    migration_id character varying(150) NOT NULL,
    product_version character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- Name: alumni; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.alumni (
    alumni_id integer NOT NULL,
    year_graduated date,
    department character varying
);


ALTER TABLE public.alumni OWNER TO postgres;

--
-- Name: attendance_register; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.attendance_register (
    attendance_id integer CONSTRAINT attendance_attendance_id_not_null NOT NULL,
    created_at timestamp without time zone CONSTRAINT attendance_scanned_at_not_null NOT NULL,
    project_id integer,
    assistant_id integer NOT NULL,
    checkinqr_token character varying NOT NULL,
    checkoutqr_token character varying
);


ALTER TABLE public.attendance_register OWNER TO postgres;

--
-- Name: attendance_attendance_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.attendance_register ALTER COLUMN attendance_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.attendance_attendance_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: attendance_entry; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.attendance_entry (
    attendance_entry_id integer NOT NULL,
    checkin_time timestamp without time zone NOT NULL,
    checkout_time timestamp without time zone,
    user_id integer NOT NULL,
    attendance_id integer NOT NULL,
    hours_logged double precision
);


ALTER TABLE public.attendance_entry OWNER TO postgres;

--
-- Name: attendance_entry_attendance_entry_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.attendance_entry ALTER COLUMN attendance_entry_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.attendance_entry_attendance_entry_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: campus; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.campus (
    campus_id integer NOT NULL,
    code character varying NOT NULL,
    name character varying NOT NULL
);


ALTER TABLE public.campus OWNER TO postgres;

--
-- Name: campus_campus_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.campus ALTER COLUMN campus_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.campus_campus_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: certificate; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.certificate (
    certificate_id integer NOT NULL,
    total_hours integer NOT NULL,
    total_projects integer NOT NULL,
    date_issued timestamp without time zone NOT NULL,
    user_id integer NOT NULL
);


ALTER TABLE public.certificate OWNER TO postgres;

--
-- Name: certificate_certificate_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.certificate ALTER COLUMN certificate_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.certificate_certificate_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: department; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.department (
    department_id integer NOT NULL,
    name character varying NOT NULL,
    faculty_id integer NOT NULL
);


ALTER TABLE public.department OWNER TO postgres;

--
-- Name: department_department_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.department ALTER COLUMN department_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.department_department_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: document; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.document (
    document_id integer NOT NULL,
    name character varying NOT NULL,
    type character varying NOT NULL,
    data bytea NOT NULL,
    size bigint,
    date_uploaded date NOT NULL,
    entity_type character varying,
    entity_id integer
);


ALTER TABLE public.document OWNER TO postgres;

--
-- Name: document_document_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.document ALTER COLUMN document_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.document_document_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: faculty; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.faculty (
    faculty_id integer NOT NULL,
    name character varying NOT NULL,
    code character varying
);


ALTER TABLE public.faculty OWNER TO postgres;

--
-- Name: faculty_faculty_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.faculty ALTER COLUMN faculty_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.faculty_faculty_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: ngo_application; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.ngo_application (
    application_id integer NOT NULL,
    name character varying CONSTRAINT ngo_application_ngo_name_not_null NOT NULL,
    email character varying CONSTRAINT ngo_application_ngo_email_not_null NOT NULL,
    close_campus character varying CONSTRAINT ngo_application_ngo_location_not_null NOT NULL,
    address character varying CONSTRAINT ngo_application_ngo_address_not_null NOT NULL,
    rep_name character varying CONSTRAINT ngo_application_representative_name_not_null NOT NULL,
    rep_position character varying CONSTRAINT ngo_application_representative_position_not_null NOT NULL,
    rep_phone character varying CONSTRAINT ngo_application_representative_phone_not_null NOT NULL,
    key_focus character varying CONSTRAINT ngo_application_ngo_key_focus_not_null NOT NULL,
    collaboration_request character varying CONSTRAINT ngo_application_obligations_description_not_null NOT NULL,
    status character varying,
    is_viewed boolean NOT NULL,
    date_submitted timestamp without time zone NOT NULL,
    specialist_id integer CONSTRAINT ngo_application_user_id_not_null NOT NULL,
    website character varying,
    reg_number character varying NOT NULL,
    description character varying,
    benefits character varying
);


ALTER TABLE public.ngo_application OWNER TO postgres;

--
-- Name: ngo_application_application_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.ngo_application ALTER COLUMN application_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.ngo_application_application_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: ngo_partnership; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.ngo_partnership (
    partnership_id integer NOT NULL,
    start_date date NOT NULL,
    renewal_date date NOT NULL,
    is_active boolean NOT NULL,
    notes character varying,
    application_id integer NOT NULL
);


ALTER TABLE public.ngo_partnership OWNER TO postgres;

--
-- Name: ngo_partnership_partnership_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.ngo_partnership ALTER COLUMN partnership_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.ngo_partnership_partnership_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: ngo_sdg; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.ngo_sdg (
    ngo_id integer NOT NULL,
    sdg_id integer NOT NULL
);


ALTER TABLE public.ngo_sdg OWNER TO postgres;

--
-- Name: notification; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.notification (
    notification_id integer NOT NULL,
    title character varying NOT NULL,
    message character varying NOT NULL,
    type character varying NOT NULL,
    created_at timestamp without time zone NOT NULL,
    user_id integer NOT NULL,
    is_read boolean NOT NULL,
    created_by_id integer
);


ALTER TABLE public.notification OWNER TO postgres;

--
-- Name: notification_notification_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.notification ALTER COLUMN notification_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.notification_notification_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: photo; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.photo (
    photo_id integer NOT NULL,
    data bytea NOT NULL,
    summary_id integer NOT NULL,
    type character varying NOT NULL
);


ALTER TABLE public.photo OWNER TO postgres;

--
-- Name: photo_photo_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.photo ALTER COLUMN photo_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.photo_photo_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: picture; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.picture (
    data bytea NOT NULL,
    picture_id integer NOT NULL,
    user_id integer NOT NULL,
    type character varying NOT NULL
);


ALTER TABLE public.picture OWNER TO postgres;

--
-- Name: picture_picture_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.picture ALTER COLUMN picture_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.picture_picture_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: project; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.project (
    project_id integer NOT NULL,
    title character varying NOT NULL,
    type character varying NOT NULL,
    location character varying NOT NULL,
    location_scope character varying NOT NULL,
    hours integer NOT NULL,
    num_volunteers integer NOT NULL,
    status character varying NOT NULL,
    category character varying NOT NULL,
    description character varying NOT NULL,
    target_group character varying NOT NULL,
    date date NOT NULL,
    date_created date NOT NULL,
    campus_id integer NOT NULL,
    user_id integer NOT NULL,
    partner character varying NOT NULL,
    partner_name character varying NOT NULL,
    partner_id character varying NOT NULL,
    is_published boolean NOT NULL,
    "time" time without time zone NOT NULL,
    slots integer
);


ALTER TABLE public.project OWNER TO postgres;

--
-- Name: project_project_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.project ALTER COLUMN project_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.project_project_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: project_register; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.project_register (
    project_id integer CONSTRAINT project_volunteering_project_id_not_null NOT NULL,
    user_id integer CONSTRAINT project_volunteering_user_id_not_null NOT NULL,
    is_committed boolean CONSTRAINT project_volunteering_status_not_null NOT NULL,
    "time" timestamp without time zone
);


ALTER TABLE public.project_register OWNER TO postgres;

--
-- Name: project_sdg; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.project_sdg (
    project_id integer NOT NULL,
    sdg_id integer NOT NULL
);


ALTER TABLE public.project_sdg OWNER TO postgres;

--
-- Name: project_summary; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.project_summary (
    summary_id integer NOT NULL,
    total_registered integer NOT NULL,
    total_volunteered integer NOT NULL,
    completed_count integer NOT NULL,
    absent_count integer NOT NULL,
    late_count integer NOT NULL,
    total_hours double precision NOT NULL,
    average_hours double precision NOT NULL,
    male_count integer,
    female_count integer,
    student_count integer NOT NULL,
    alumni_count integer NOT NULL,
    completion_notes character varying NOT NULL,
    generated_at timestamp without time zone NOT NULL,
    project_id integer NOT NULL
);


ALTER TABLE public.project_summary OWNER TO postgres;

--
-- Name: project_summary_summary_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.project_summary ALTER COLUMN summary_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.project_summary_summary_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: qrcode; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.qrcode (
    qrcode_id integer NOT NULL,
    qrcode_token character varying NOT NULL,
    time_expires timestamp without time zone NOT NULL,
    user_id integer NOT NULL,
    project_id integer NOT NULL
);


ALTER TABLE public.qrcode OWNER TO postgres;

--
-- Name: qrcode_qrcode_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.qrcode ALTER COLUMN qrcode_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.qrcode_qrcode_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: report; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.report (
    report_id integer NOT NULL,
    notes character varying NOT NULL,
    status character varying NOT NULL,
    date_created timestamp without time zone NOT NULL,
    user_id integer NOT NULL,
    project_id integer NOT NULL
);


ALTER TABLE public.report OWNER TO postgres;

--
-- Name: report_report_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.report ALTER COLUMN report_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.report_report_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: sdg; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.sdg (
    sdg_id integer NOT NULL,
    name character varying NOT NULL
);


ALTER TABLE public.sdg OWNER TO postgres;

--
-- Name: sdg_sdg_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.sdg ALTER COLUMN sdg_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.sdg_sdg_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: sponsor; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.sponsor (
    sponsor_id integer NOT NULL,
    name character varying NOT NULL,
    address character varying NOT NULL,
    telephone character varying NOT NULL,
    pledge integer NOT NULL
);


ALTER TABLE public.sponsor OWNER TO postgres;

--
-- Name: sponsor_sponsor_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.sponsor ALTER COLUMN sponsor_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.sponsor_sponsor_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: staff; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.staff (
    staff_id integer NOT NULL,
    department character varying NOT NULL
);


ALTER TABLE public.staff OWNER TO postgres;

--
-- Name: student; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.student (
    student_id integer NOT NULL,
    student_number character varying,
    year_of_study character varying,
    qualification character varying,
    department character varying
);


ALTER TABLE public.student OWNER TO postgres;

--
-- Name: suspension; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.suspension (
    suspension_id integer NOT NULL,
    start_date date NOT NULL,
    end_date date NOT NULL,
    is_active boolean NOT NULL,
    user_id integer NOT NULL
);


ALTER TABLE public.suspension OWNER TO postgres;

--
-- Name: suspension_suspension_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.suspension ALTER COLUMN suspension_id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.suspension_suspension_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: unregistered_volunteer; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.unregistered_volunteer (
    id integer NOT NULL,
    email character varying NOT NULL,
    name character varying NOT NULL,
    surname character varying NOT NULL,
    campus_id integer NOT NULL,
    hours double precision NOT NULL
);


ALTER TABLE public.unregistered_volunteer OWNER TO postgres;

--
-- Name: unregistered_volunteer_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.unregistered_volunteer ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.unregistered_volunteer_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: user_demographics; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.user_demographics (
    user_id integer CONSTRAINT userdemographics_user_id_not_null NOT NULL,
    geo_area character varying CONSTRAINT userdemographics_geo_area_not_null NOT NULL,
    home_language character varying CONSTRAINT userdemographics_home_language_not_null NOT NULL,
    gender character varying CONSTRAINT userdemographics_gender_not_null NOT NULL,
    is_international boolean CONSTRAINT userdemographics_isinternational_not_null NOT NULL
);


ALTER TABLE public.user_demographics OWNER TO postgres;

--
-- Name: volunteer; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.volunteer (
    volunteer_id integer NOT NULL,
    volunteer_type character varying NOT NULL,
    is_completed boolean CONSTRAINT volunteer_issuspended_not_null NOT NULL,
    recruiter_id integer,
    hours double precision NOT NULL
);


ALTER TABLE public.volunteer OWNER TO postgres;

--
-- Name: volunteer_sdg; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.volunteer_sdg (
    volunteer_id integer CONSTRAINT user_sdg_user_id_not_null NOT NULL,
    sdg_id integer CONSTRAINT user_sdg_sdg_id_not_null NOT NULL
);


ALTER TABLE public.volunteer_sdg OWNER TO postgres;

--
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."__EFMigrationsHistory" (migration_id, product_version) FROM stdin;
\.


--
-- Data for Name: alumni; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.alumni (alumni_id, year_graduated, department) FROM stdin;
\.


--
-- Data for Name: attendance_entry; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.attendance_entry (attendance_entry_id, checkin_time, checkout_time, user_id, attendance_id, hours_logged) FROM stdin;
\.


--
-- Data for Name: attendance_register; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.attendance_register (attendance_id, created_at, project_id, assistant_id, checkinqr_token, checkoutqr_token) FROM stdin;
\.


--
-- Data for Name: campus; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.campus (campus_id, code, name) FROM stdin;
1	APK	Auckland Park
2	APB	Bunting Road
3	DFC	Doorfontein
4	SWC	Soweto
\.


--
-- Data for Name: certificate; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.certificate (certificate_id, total_hours, total_projects, date_issued, user_id) FROM stdin;
\.


--
-- Data for Name: department; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.department (department_id, name, faculty_id) FROM stdin;
1	Department of Business Management	1
2	Department of Finance and Investment Management	1
3	Department of Industrial Psychology and People Management	1
4	Department of Transport and Supply Chain Management	1
5	Department of Accountancy	1
6	Department of Commercial Accounting	1
7	Department of Applied Information Systems	1
8	Department of Information and Knowledge Management	1
9	Department of Marketing Management	1
10	Economics And Econometrics	1
11	Department of Public Management and Governance	1
12	School of Tourism and Hospitality	1
13	Department of Architecture	2
14	Department of Fashion Design	2
15	Department of Graphic Design	2
16	Department of Industrial Design	2
17	Department of Interior Design	2
18	Department of Jewellery Design and Manufacture	2
19	Department of Multimedia	2
20	Department of Visual Art	2
21	Department of Childhood Education	3
22	Department of Education and Curriculum Studies	3
23	Department of Education Leadership and Management	3
24	Department of Educational Psychology	3
25	Department of Science and Technology Education	3
26	School of Electrical Engineering and Electronic Engineering	4
27	School of Civil Engineering and the Built Environment	4
28	School of Mechanical and Industrial Engineering	4
29	School of Mining, Metallurgy and Chemical Engineering	4
30	Postgraduate School of Engineering Management	4
31	Department of Environmental Health	5
32	Department of Human Anatomy and Physiology	5
33	Department of Nursing	5
34	Department of Optometry	5
35	Department of Podiatry	5
36	Department of Emergency Medical Care	5
37	Department of Medical Imaging and Radiation Sciences (Radiography)	5
38	Department of Homoeopathy	5
39	Department of Sport and Movement Studies	5
40	Department of Somatology	5
41	Department of Chiropractic	5
42	Department of Biomedical Technology	5
43	Department of African Languages	6
44	Department of Greek and Latin Studies	6
45	Department of Afrikaans	6
46	Department of Historical Studies	6
47	Department of Anthropology and Development Studies	6
48	Department of Linguistics	6
49	School of Communication	6
50	Department of Philosophy	6
51	Department of Communication Studies	6
52	Department of Politics and International Relations	6
53	Department of Strategic Communication	6
54	Department of Psychology	6
55	Department of English	6
56	Department of Religion Studies	6
57	Department of French	6
58	Department of Social Work	6
59	Department of Journalism, Film and Television	6
60	Department of Sociology	6
61	Department of Mercantile Law	7
62	Department of Practical Business Law	7
63	Department of Private Law	7
64	Department of Procedural Law	7
65	Department of Public Law	7
66	Department of Law Clinic	7
67	Academy of Computer Science and Software Engineering	8
68	Department of Applied Chemistry	8
69	Department of Applied Physics and Engineering Mathematics	8
70	Department of Biochemistry	8
71	Department of Biotechnology and Food Technology	8
72	Department of Botany and Plant Biotechnology	8
73	Department of Chemistry	8
74	Department of Geography	8
75	Department of Geology	8
76	Department of Physics	8
77	Department of Pure and Applied Mathematics	8
78	Department of Statistics	8
79	Department of Zoology	8
80	Applied Information Systems	9
81	Business Management	9
82	Centre for Small Business Development	9
83	Centre for Social Entrepreneurship and Social Economy\n	9
84	Industrial Psychology and People Management	9
85	Information and Knowledge Management	9
86	Marketing Management	9
87	Public Management and Governance	9
88	School of Leadership	9
89	School of Tourism and Hospitality Management	9
90	Transport and Supply Chain Management	9
\.


--
-- Data for Name: document; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.document (document_id, name, type, data, size, date_uploaded, entity_type, entity_id) FROM stdin;
\.


--
-- Data for Name: faculty; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.faculty (faculty_id, name, code) FROM stdin;
1	College of Business and Economics	CBE
2	Faculty of Art, Design And Architecture	FADA
3	Faculty of Education	EDU
4	Faculty of Engineering & the Built Environment	FEBE
5	Faculty of Health Sciences	HS
6	Faculty of Humanities	HUM
7	Faculty of Law	LAW
8	Faculty of Science	SCI
9	Johannesburg Business School	JBS
\.


--
-- Data for Name: ngo_application; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.ngo_application (application_id, name, email, close_campus, address, rep_name, rep_position, rep_phone, key_focus, collaboration_request, status, is_viewed, date_submitted, specialist_id, website, reg_number, description, benefits) FROM stdin;
\.


--
-- Data for Name: ngo_partnership; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.ngo_partnership (partnership_id, start_date, renewal_date, is_active, notes, application_id) FROM stdin;
\.


--
-- Data for Name: ngo_sdg; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.ngo_sdg (ngo_id, sdg_id) FROM stdin;
\.


--
-- Data for Name: notification; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.notification (notification_id, title, message, type, created_at, user_id, is_read, created_by_id) FROM stdin;
\.


--
-- Data for Name: photo; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.photo (photo_id, data, summary_id, type) FROM stdin;
\.


--
-- Data for Name: picture; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.picture (data, picture_id, user_id, type) FROM stdin;
\.


--
-- Data for Name: project; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.project (project_id, title, type, location, location_scope, hours, num_volunteers, status, category, description, target_group, date, date_created, campus_id, user_id, partner, partner_name, partner_id, is_published, "time", slots) FROM stdin;
\.


--
-- Data for Name: project_register; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.project_register (project_id, user_id, is_committed, "time") FROM stdin;
\.


--
-- Data for Name: project_sdg; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.project_sdg (project_id, sdg_id) FROM stdin;
\.


--
-- Data for Name: project_summary; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.project_summary (summary_id, total_registered, total_volunteered, completed_count, absent_count, late_count, total_hours, average_hours, male_count, female_count, student_count, alumni_count, completion_notes, generated_at, project_id) FROM stdin;
\.


--
-- Data for Name: qrcode; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.qrcode (qrcode_id, qrcode_token, time_expires, user_id, project_id) FROM stdin;
\.


--
-- Data for Name: report; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.report (report_id, notes, status, date_created, user_id, project_id) FROM stdin;
\.


--
-- Data for Name: sdg; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.sdg (sdg_id, name) FROM stdin;
1	No Poverty
2	Zero Hunger
3	Good Health & Well-being
4	Quality Education
5	Gender Equality
6	Clean Water & Sanitation
7	Affordable & Clean Energy
8	Decent Work & Economic Growth
9	Industry, Innovation & Infrastructure
10	Reduced Inequalities
11	Sustainable Cities & Communities
12	Responsible Consumption & Production
13	Climate Action
14	Life Below Water
15	Life On Land
16	Peace, Justice & Strong Institutions
17	Partnerships for the Goals
\.


--
-- Data for Name: sponsor; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.sponsor (sponsor_id, name, address, telephone, pledge) FROM stdin;
\.


--
-- Data for Name: staff; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.staff (staff_id, department) FROM stdin;
\.


--
-- Data for Name: student; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.student (student_id, student_number, year_of_study, qualification, department) FROM stdin;
\.


--
-- Data for Name: suspension; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.suspension (suspension_id, start_date, end_date, is_active, user_id) FROM stdin;
\.


--
-- Data for Name: unregistered_volunteer; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.unregistered_volunteer (id, email, name, surname, campus_id, hours) FROM stdin;
\.


--
-- Data for Name: user; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."user" (user_id, first_name, last_name, email, phone, id_number, password, date_created, campus_id, is_active, is_suspended, role, faculty_id) FROM stdin;
1	Director	Fiona	director@uj.ac.za	+0000000001	#UJ-1	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2026-09-26	1	t	f	director	1
2	Specialist	Thabo	apk.specialist@uj.ac.za	+0000000002	#UJ-2	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2026-09-27	1	t	f	specialist	1
3	Specialist	Naledi	apb.specialist@uj.ac.za	+0000000003	#UJ-3	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2026-09-27	2	t	f	specialist	1
4	Specialist	Zakumi	dfc.specialist@uj.ac.za	+0000000004	#UJ-4	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2026-09-27	3	t	f	specialist	1
5	Specialist	Kalito	swc.specialist@uj.ac.za	+0000000005	#UJ-5	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2026-09-27	4	t	f	specialist	1
\.


--
-- Data for Name: user_demographics; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.user_demographics (user_id, geo_area, home_language, gender, is_international) FROM stdin;
\.


--
-- Data for Name: volunteer; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.volunteer (volunteer_id, volunteer_type, is_completed, recruiter_id, hours) FROM stdin;
\.


--
-- Data for Name: volunteer_sdg; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.volunteer_sdg (volunteer_id, sdg_id) FROM stdin;
\.


--
-- Name: User_user_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."User_user_id_seq"', 5, true);


--
-- Name: attendance_attendance_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.attendance_attendance_id_seq', 1, false);


--
-- Name: attendance_entry_attendance_entry_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.attendance_entry_attendance_entry_id_seq', 1, false);


--
-- Name: campus_campus_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.campus_campus_id_seq', 4, true);


--
-- Name: certificate_certificate_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.certificate_certificate_id_seq', 1, false);


--
-- Name: department_department_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.department_department_id_seq', 90, true);


--
-- Name: document_document_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.document_document_id_seq', 1, false);


--
-- Name: faculty_faculty_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.faculty_faculty_id_seq', 9, true);


--
-- Name: ngo_application_application_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.ngo_application_application_id_seq', 1, false);


--
-- Name: ngo_partnership_partnership_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.ngo_partnership_partnership_id_seq', 1, false);


--
-- Name: notification_notification_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.notification_notification_id_seq', 1, false);


--
-- Name: photo_photo_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.photo_photo_id_seq', 1, false);


--
-- Name: picture_picture_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.picture_picture_id_seq', 1, false);


--
-- Name: project_project_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.project_project_id_seq', 1, false);


--
-- Name: project_summary_summary_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.project_summary_summary_id_seq', 1, false);


--
-- Name: qrcode_qrcode_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.qrcode_qrcode_id_seq', 1, false);


--
-- Name: report_report_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.report_report_id_seq', 1, false);


--
-- Name: sdg_sdg_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.sdg_sdg_id_seq', 17, true);


--
-- Name: sponsor_sponsor_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.sponsor_sponsor_id_seq', 1, false);


--
-- Name: suspension_suspension_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.suspension_suspension_id_seq', 1, false);


--
-- Name: unregistered_volunteer_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.unregistered_volunteer_id_seq', 1, false);


--
-- Name: alumni alumni_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.alumni
    ADD CONSTRAINT alumni_pk PRIMARY KEY (alumni_id);


--
-- Name: attendance_entry attendance_entry_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.attendance_entry
    ADD CONSTRAINT attendance_entry_pk PRIMARY KEY (attendance_entry_id);


--
-- Name: attendance_register attendance_register_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.attendance_register
    ADD CONSTRAINT attendance_register_pk PRIMARY KEY (attendance_id);


--
-- Name: campus campus_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.campus
    ADD CONSTRAINT campus_pk PRIMARY KEY (campus_id);


--
-- Name: certificate certificate_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.certificate
    ADD CONSTRAINT certificate_pk PRIMARY KEY (certificate_id);


--
-- Name: department department_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.department
    ADD CONSTRAINT department_pk PRIMARY KEY (department_id);


--
-- Name: document document_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.document
    ADD CONSTRAINT document_pk PRIMARY KEY (document_id);


--
-- Name: faculty faculty_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.faculty
    ADD CONSTRAINT faculty_pk PRIMARY KEY (faculty_id);


--
-- Name: ngo_application ngo_application_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ngo_application
    ADD CONSTRAINT ngo_application_pk PRIMARY KEY (application_id);


--
-- Name: ngo_partnership ngo_partnership_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ngo_partnership
    ADD CONSTRAINT ngo_partnership_pk PRIMARY KEY (partnership_id);


--
-- Name: ngo_sdg ngo_sdg_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ngo_sdg
    ADD CONSTRAINT ngo_sdg_pk PRIMARY KEY (ngo_id, sdg_id);


--
-- Name: notification notification_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.notification
    ADD CONSTRAINT notification_pk PRIMARY KEY (notification_id);


--
-- Name: picture picture_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.picture
    ADD CONSTRAINT picture_pk PRIMARY KEY (picture_id);


--
-- Name: __EFMigrationsHistory pk___ef_migrations_history; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT pk___ef_migrations_history PRIMARY KEY (migration_id);


--
-- Name: project project_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.project
    ADD CONSTRAINT project_id PRIMARY KEY (project_id);


--
-- Name: project_sdg project_sdg_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.project_sdg
    ADD CONSTRAINT project_sdg_pkey PRIMARY KEY (project_id, sdg_id);


--
-- Name: project_summary project_summary_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.project_summary
    ADD CONSTRAINT project_summary_pk PRIMARY KEY (summary_id);


--
-- Name: project_register project_volunteering_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.project_register
    ADD CONSTRAINT project_volunteering_pkey PRIMARY KEY (project_id, user_id);


--
-- Name: qrcode qrcode_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.qrcode
    ADD CONSTRAINT qrcode_pk PRIMARY KEY (qrcode_id);


--
-- Name: report report_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.report
    ADD CONSTRAINT report_pk PRIMARY KEY (report_id);


--
-- Name: sdg sdg_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sdg
    ADD CONSTRAINT sdg_pk PRIMARY KEY (sdg_id);


--
-- Name: staff staff_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.staff
    ADD CONSTRAINT staff_pk PRIMARY KEY (staff_id);


--
-- Name: student student_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.student
    ADD CONSTRAINT student_pk PRIMARY KEY (student_id);


--
-- Name: suspension suspension_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.suspension
    ADD CONSTRAINT suspension_pk PRIMARY KEY (suspension_id);


--
-- Name: unregistered_volunteer unregistered_volunteer_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.unregistered_volunteer
    ADD CONSTRAINT unregistered_volunteer_pk PRIMARY KEY (id);


--
-- Name: user_demographics user_demographics_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_demographics
    ADD CONSTRAINT user_demographics_pk PRIMARY KEY (user_id);


--
-- Name: user user_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."user"
    ADD CONSTRAINT user_pk PRIMARY KEY (user_id);


--
-- Name: volunteer_sdg user_sdg_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.volunteer_sdg
    ADD CONSTRAINT user_sdg_pkey PRIMARY KEY (volunteer_id, sdg_id);


--
-- Name: volunteer volunteer_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.volunteer
    ADD CONSTRAINT volunteer_pk PRIMARY KEY (volunteer_id);


--
-- Name: user User_campus_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."user"
    ADD CONSTRAINT "User_campus_id_fkey" FOREIGN KEY (campus_id) REFERENCES public.campus(campus_id);


--
-- Name: alumni alumni___fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.alumni
    ADD CONSTRAINT alumni___fk FOREIGN KEY (alumni_id) REFERENCES public.volunteer(volunteer_id);


--
-- Name: attendance_entry attendance_entry___fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.attendance_entry
    ADD CONSTRAINT attendance_entry___fk FOREIGN KEY (user_id) REFERENCES public."user"(user_id);


--
-- Name: attendance_entry attendance_entry___fk_2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.attendance_entry
    ADD CONSTRAINT attendance_entry___fk_2 FOREIGN KEY (attendance_id) REFERENCES public.attendance_register(attendance_id);


--
-- Name: attendance_register attendance_register___fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.attendance_register
    ADD CONSTRAINT attendance_register___fk FOREIGN KEY (project_id) REFERENCES public.project(project_id);


--
-- Name: attendance_register attendance_register___fk_3; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.attendance_register
    ADD CONSTRAINT attendance_register___fk_3 FOREIGN KEY (assistant_id) REFERENCES public."user"(user_id);


--
-- Name: certificate certificate___fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.certificate
    ADD CONSTRAINT certificate___fk FOREIGN KEY (user_id) REFERENCES public."user"(user_id);


--
-- Name: department department___fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.department
    ADD CONSTRAINT department___fk FOREIGN KEY (faculty_id) REFERENCES public.faculty(faculty_id);


--
-- Name: ngo_application ngo_application___fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ngo_application
    ADD CONSTRAINT ngo_application___fk FOREIGN KEY (specialist_id) REFERENCES public."user"(user_id);


--
-- Name: ngo_partnership ngo_partnership___fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ngo_partnership
    ADD CONSTRAINT ngo_partnership___fk FOREIGN KEY (application_id) REFERENCES public.ngo_application(application_id);


--
-- Name: ngo_sdg ngo_sdg___fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ngo_sdg
    ADD CONSTRAINT ngo_sdg___fk FOREIGN KEY (ngo_id) REFERENCES public.ngo_application(application_id);


--
-- Name: ngo_sdg ngo_sdg___fk_2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ngo_sdg
    ADD CONSTRAINT ngo_sdg___fk_2 FOREIGN KEY (sdg_id) REFERENCES public.sdg(sdg_id);


--
-- Name: notification notification___fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.notification
    ADD CONSTRAINT notification___fk FOREIGN KEY (user_id) REFERENCES public."user"(user_id);


--
-- Name: picture picture___fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.picture
    ADD CONSTRAINT picture___fk FOREIGN KEY (user_id) REFERENCES public."user"(user_id);


--
-- Name: project project___fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.project
    ADD CONSTRAINT project___fk FOREIGN KEY (campus_id) REFERENCES public.campus(campus_id);


--
-- Name: project project___fk_2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.project
    ADD CONSTRAINT project___fk_2 FOREIGN KEY (user_id) REFERENCES public."user"(user_id);


--
-- Name: project_sdg project_sdg_project_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.project_sdg
    ADD CONSTRAINT project_sdg_project_id_fkey FOREIGN KEY (project_id) REFERENCES public.project(project_id);


--
-- Name: project_sdg project_sdg_sdg_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.project_sdg
    ADD CONSTRAINT project_sdg_sdg_id_fkey FOREIGN KEY (sdg_id) REFERENCES public.sdg(sdg_id);


--
-- Name: project_summary project_summary___fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.project_summary
    ADD CONSTRAINT project_summary___fk FOREIGN KEY (project_id) REFERENCES public.project(project_id);


--
-- Name: project_register project_volunteering_project_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.project_register
    ADD CONSTRAINT project_volunteering_project_id_fkey FOREIGN KEY (project_id) REFERENCES public.project(project_id);


--
-- Name: project_register project_volunteering_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.project_register
    ADD CONSTRAINT project_volunteering_user_id_fkey FOREIGN KEY (user_id) REFERENCES public."user"(user_id);


--
-- Name: qrcode qrcode___fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.qrcode
    ADD CONSTRAINT qrcode___fk FOREIGN KEY (user_id) REFERENCES public."user"(user_id);


--
-- Name: qrcode qrcode___fk_2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.qrcode
    ADD CONSTRAINT qrcode___fk_2 FOREIGN KEY (project_id) REFERENCES public.project(project_id);


--
-- Name: report report___fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.report
    ADD CONSTRAINT report___fk FOREIGN KEY (user_id) REFERENCES public."user"(user_id);


--
-- Name: report report___fk_2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.report
    ADD CONSTRAINT report___fk_2 FOREIGN KEY (project_id) REFERENCES public.project(project_id);


--
-- Name: staff staff___fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.staff
    ADD CONSTRAINT staff___fk FOREIGN KEY (staff_id) REFERENCES public.volunteer(volunteer_id);


--
-- Name: student student_student_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.student
    ADD CONSTRAINT student_student_id_fkey FOREIGN KEY (student_id) REFERENCES public.volunteer(volunteer_id);


--
-- Name: suspension suspension___fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.suspension
    ADD CONSTRAINT suspension___fk FOREIGN KEY (user_id) REFERENCES public."user"(user_id);


--
-- Name: user user___fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."user"
    ADD CONSTRAINT user___fk FOREIGN KEY (faculty_id) REFERENCES public.faculty(faculty_id);


--
-- Name: volunteer_sdg user_sdg_sdg_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.volunteer_sdg
    ADD CONSTRAINT user_sdg_sdg_id_fkey FOREIGN KEY (sdg_id) REFERENCES public.sdg(sdg_id);


--
-- Name: volunteer_sdg user_sdg_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.volunteer_sdg
    ADD CONSTRAINT user_sdg_user_id_fkey FOREIGN KEY (volunteer_id) REFERENCES public.volunteer(volunteer_id);


--
-- Name: user_demographics userdemographics_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_demographics
    ADD CONSTRAINT userdemographics_user_id_fkey FOREIGN KEY (user_id) REFERENCES public."user"(user_id);


--
-- Name: volunteer volunteer_recruiter_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.volunteer
    ADD CONSTRAINT volunteer_recruiter_id_fkey FOREIGN KEY (recruiter_id) REFERENCES public."user"(user_id);


--
-- Name: volunteer volunteer_volunteer_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.volunteer
    ADD CONSTRAINT volunteer_volunteer_id_fkey FOREIGN KEY (volunteer_id) REFERENCES public."user"(user_id);


--
-- PostgreSQL database dump complete
--



