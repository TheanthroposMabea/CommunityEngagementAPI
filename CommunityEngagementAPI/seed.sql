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
    is_committed boolean CONSTRAINT project_volunteering_status_not_null NOT NULL
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
31	2021-12-10	Department of Psychology
32	2020-12-10	Academy of Computer Science and Software Engineering
33	2022-12-10	Department of Marketing Management
34	2019-12-10	Department of Finance and Investment Management
35	2021-12-10	Department of Social Work
36	2018-12-10	Department of Public Management and Governance
\.


--
-- Data for Name: attendance_entry; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.attendance_entry (attendance_entry_id, checkin_time, checkout_time, user_id, attendance_id, hours_logged) FROM stdin;
1	2026-07-31 09:00:00	2026-07-31 13:00:00	26	1	4
2	2026-07-31 09:00:00	2026-07-31 12:30:00	27	1	3.5
3	2026-07-31 09:00:00	2026-07-31 13:00:00	31	1	4
4	2026-07-31 09:00:00	2026-07-31 11:00:00	33	1	2
5	2026-08-01 10:00:00	2026-08-02 04:00:00	26	2	18
6	2026-08-01 10:00:00	2026-08-02 03:00:00	28	2	17
7	2026-08-01 10:00:00	2026-08-02 06:00:00	29	2	20
8	2026-08-01 10:00:00	2026-08-02 01:00:00	32	2	15
9	2026-07-15 14:00:00	2026-07-17 04:00:00	26	3	38
10	2026-07-15 14:00:00	2026-07-16 14:00:00	27	3	24
11	2026-07-15 14:00:00	2026-07-17 06:00:00	29	3	40
12	2026-07-15 14:00:00	2026-07-16 12:00:00	30	3	22
13	2026-07-15 14:00:00	2026-07-16 16:00:00	35	3	26
14	2026-07-20 08:30:00	2026-07-20 16:30:00	26	4	8
15	2026-07-20 08:30:00	2026-07-20 16:30:00	29	4	8
16	2026-07-20 08:30:00	2026-07-20 15:30:00	34	4	7
17	2026-07-25 09:00:00	2026-07-25 15:00:00	26	5	6
18	2026-07-25 09:00:00	2026-07-25 14:30:00	28	5	5.5
19	2026-07-25 09:00:00	2026-07-25 15:00:00	31	5	6
20	2026-08-10 11:00:00	2026-08-11 07:00:00	26	6	20
21	2026-08-10 11:00:00	2026-08-11 07:00:00	29	6	20
22	2026-08-05 08:00:00	2026-08-05 13:00:00	26	7	5
23	2026-08-05 08:00:00	2026-08-05 12:30:00	27	7	4.5
24	2026-08-05 08:00:00	2026-08-05 13:00:00	28	7	5
25	2026-08-05 08:00:00	2026-08-05 13:00:00	35	7	5
26	2026-08-08 13:00:00	2026-08-09 04:00:00	26	8	15
27	2026-08-08 13:00:00	2026-08-09 04:00:00	29	8	15
28	2026-08-08 13:00:00	2026-08-08 22:00:00	30	8	9
29	2026-08-08 13:00:00	2026-08-08 23:00:00	33	8	10
\.


--
-- Data for Name: attendance_register; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.attendance_register (attendance_id, created_at, project_id, assistant_id, checkinqr_token, checkoutqr_token) FROM stdin;
1	2026-07-30 08:00:00	1	18	CHK-1-35	OUT-1-35
2	2026-07-30 08:00:00	2	18	CHK-1-38	OUT-1-38
3	2026-07-30 08:00:00	3	19	CHK-2-33	OUT-2-33
4	2026-07-30 08:00:00	4	19	CHK-2-34	OUT-2-34
5	2026-07-30 08:00:00	5	20	CHK-3-33	OUT-3-33
6	2026-07-30 08:00:00	6	20	CHK-3-35	OUT-3-35
7	2026-07-30 08:00:00	7	21	CHK-4-29	OUT-4-29
8	2026-07-30 08:00:00	8	21	CHK-4-40	OUT-4-40
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
1	114	8	2026-08-19 03:34:31.809109	26
2	103	6	2026-08-19 03:34:31.809109	29
3	100	5	2026-08-24 21:30:14	3
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
1	Alexandra Community Trust	info@alexcommunitytrust.org.za	APK	12 Roosevelt St, Alexandra, Johannesburg	Nomsa Khoza	Programme Director	+27824000001	Poverty alleviation	Volunteer placements for community surveys	PendingReview	f	2026-06-01 09:12:00	2	https://alexcommunitytrust.org.za	2019/123456/08	NGO supporting low-income households in Alexandra with food and clothing drives.	Access to student volunteers for fieldwork and logistics support.
2	Bunting Community Garden	hello@buntinggarden.org.za	APB	22 Main Road, Bunting Road, Johannesburg	Lerato Mokoena	Coordinator	+27824000002	Food security	Student volunteers for garden maintenance	Approved	t	2026-05-15 10:30:00	7	https://buntinggarden.org.za	2018/445566/08	Community food garden and nutrition programme.	Volunteer labour and student research support.
4	FuturePath Network	contact@futurepath.org.za	SWC	41 Vilakazi Street, Soweto	Thandi Maseko	Partnership Lead	+27824000004	Youth development	Career readiness and mentoring collaboration	Approved	t	2026-05-25 14:00:00	15	https://futurepath.org.za	2020/778899/08	Youth employability and mentorship organisation.	Student mentors and workshop facilitators.
5	Code4Community	hello@code4community.org.za	APK	7 University Road, Auckland Park	Sello Molefe	Director	+27824000005	Digital skills	Coding workshop collaboration	MissingInformation	f	2026-06-10 08:45:00	4	\N	2021/112233/08	Technology education initiative for young people.	Student volunteers and specialist mentorship.
3	HealthReach Gauteng	info@healthreach.org.za	DFC	9 Commissioner Street, Johannesburg	Dr Kabelo Dube	Programme Manager	+27824000003	Public health	Health awareness and screening volunteers	Approved	t	2026-05-20 11:00:00	10	https://healthreach.org.za	2017/334455/08	Community wellness and preventative health initiatives.	Volunteer support during outreach events.
\.


--
-- Data for Name: ngo_partnership; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.ngo_partnership (partnership_id, start_date, renewal_date, is_active, notes, application_id) FROM stdin;
1	2026-06-01	2027-06-01	t	Active food security partnership.	2
2	2026-06-05	2027-06-05	t	Health outreach partnership.	3
3	2026-06-10	2027-06-10	t	Youth career readiness partnership.	4
\.


--
-- Data for Name: ngo_sdg; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.ngo_sdg (ngo_id, sdg_id) FROM stdin;
1	1
1	10
2	2
2	11
2	12
3	3
3	10
4	4
4	8
5	4
5	9
\.


--
-- Data for Name: notification; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.notification (notification_id, title, message, type, created_at, user_id, is_read, created_by_id) FROM stdin;
1	Project joined	You successfully joined the Winter Clothing Drive for Alexandra.	ProjectJoined	2026-07-20 09:00:00	26	f	1
2	Attendance confirmed	Your attendance was recorded for the Soweto Youth Mentorship Programme.	AttendanceConfirmed	2026-07-15 17:00:00	26	f	18
3	Certificate issued	You have exceeded 100 volunteer hours and earned a certificate.	CertificateIssued	2026-08-12 15:00:00	26	f	\N
4	NGO application submitted	A new NGO application is awaiting review.	ApplicationSubmitted	2026-06-01 09:15:00	1	f	2
5	Project created	A new community project has been created at your campus.	ProjectCreated	2026-07-20 10:00:00	21	f	14
6	General update	Please review outstanding community engagement reports.	GeneralAdmin	2026-08-15 08:00:00	22	f	1
7	Joined Project	You have successfully joined the project Digital Literacy Workshops for Seniors. Please look out for the Project date - 2026/09/19	ProjectJoined	2026-09-14 23:01:00.349954	4	f	\N
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
4	Community Garden and Food Security	Outreach	Bunting Road, Johannesburg	Local	8	12	Completed	OrganisedOutreach	Build food gardens and train households in sustainable food production.	Food-insecure households	2026-07-20	2026-01-05	1	2	CBO	Bunting Community Garden	2018/445566/08	t	08:30:00	12
1	Winter Clothing Drive for Alexandra	Outreach	Alexandra, Johannesburg	Local	4	15	Completed	CommunityBasedResearch	Collect and distribute winter clothing to households identified in the Alexandra ward survey.	Low-income households	2026-07-31	2026-01-05	1	2	CBO	Alexandra Community Trust	2013/901234/08	t	09:00:00	15
5	Health Screening and Wellness Day	Outreach	Doorfontein, Johannesburg	Regional	6	18	Completed	ServiceLearning	Provide basic health screening, wellness education and referral information.	Local residents	2026-07-25	2026-01-05	1	2	NPO	HealthReach Gauteng	2017/334455/08	t	09:00:00	18
10	Youth Coding and Innovation Bootcamp	Workshop	Bunting Road, Johannesburg	Regional	16	12	Cancelled	ServiceLearning	Coding and innovation workshops for young people.	High-school learners	2026-08-19	2026-01-05	2	6	NPO	Code4Community	2021/112233/08	f	13:30:00	12
8	Career Readiness for Final-Year Students	Workshop	Soweto, Johannesburg	National	5	16	Completed	ServiceLearning	CV writing, interview preparation and workplace readiness sessions.	Final-year students	2026-08-08	2026-01-05	4	14	NPO	FuturePath Network	2020/778899/08	t	13:00:00	16
3	Soweto Youth Mentorship Programme	Mentorship	Soweto, Johannesburg	Local	2	20	Completed	OrganisedOutreach	Pairs student volunteers with at-risk youth for weekly mentoring sessions.	At-risk youth	2026-07-15	2026-01-05	1	2	DEPARTMENT	Dept. of Social Development	2019/123456/08	t	14:00:00	20
7	Soweto Environmental Clean-up	Outreach	Soweto, Johannesburg	Local	5	25	Completed	OrganisedOutreach	Community clean-up focused on litter removal and recycling education.	Youth and residents	2026-08-05	2026-01-05	4	14	DEPARTMENT	City Environmental Unit	2019/123456/08	t	08:00:00	25
6	Digital Skills for Small Businesses	Workshop	Doorfontein, Johannesburg	Regional	8	10	Pending	ServiceLearning	Help small businesses improve digital record keeping and online presence.	Small business owners	2026-09-19	2026-01-05	3	10	CBO	UJ Enterprise Support	2018/445566/08	t	14:00:00	10
9	Water Conservation Awareness Campaign	Outreach	Alexandra, Johannesburg	Local	7	14	Pending	CommunityBasedResearch	Community education campaign on household water conservation.	Households	2026-09-15	2026-01-05	1	2	CBO	Alexandra Water Forum	2016/998877/08	t	09:00:00	14
2	Digital Literacy Workshops for Seniors	Workshop	Soweto, Johannesburg	Regional	5	8	Pending	ServiceLearning	Weekly workshops teaching basic smartphone and internet skills to elderly residents.	Senior citizens	2026-09-19	2026-01-05	1	2	NPO	Age-in-Action SA	2012/012345/08	t	14:00:00	8
\.


--
-- Data for Name: project_register; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.project_register (project_id, user_id, is_committed) FROM stdin;
1	26	t
1	27	t
1	31	t
1	33	t
2	26	t
2	28	t
2	29	t
2	32	t
3	26	t
3	27	t
3	29	t
3	30	t
3	35	t
4	26	t
4	29	t
4	34	t
5	26	t
5	28	t
5	31	t
6	26	t
6	29	t
7	26	t
7	27	t
7	28	t
7	35	t
8	26	t
8	29	t
8	30	t
8	33	t
10	29	t
2	4	t
\.


--
-- Data for Name: project_sdg; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.project_sdg (project_id, sdg_id) FROM stdin;
1	1
1	10
1	12
2	4
2	10
3	4
3	10
3	16
4	2
4	11
4	12
4	13
5	3
5	10
6	8
6	9
7	11
7	12
7	13
7	15
8	4
8	8
9	6
9	11
9	12
9	13
10	4
10	9
10	10
\.


--
-- Data for Name: project_summary; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.project_summary (summary_id, total_registered, total_volunteered, completed_count, absent_count, late_count, total_hours, average_hours, male_count, female_count, student_count, alumni_count, completion_notes, generated_at, project_id) FROM stdin;
1	4	4	4	0	0	54	3.375	1	3	2	2	Seeded summary generated from attendance data.	2026-08-19 03:34:31.809109	1
2	4	4	4	0	0	280	17.5	3	1	3	1	Seeded summary generated from attendance data.	2026-08-19 03:34:31.809109	2
3	5	5	5	0	0	750	30	3	2	4	1	Seeded summary generated from attendance data.	2026-08-19 03:34:31.809109	3
4	3	3	3	0	0	69	7.666666666666667	2	1	2	1	Seeded summary generated from attendance data.	2026-08-19 03:34:31.809109	4
5	3	3	3	0	0	52.5	5.833333333333333	1	2	2	1	Seeded summary generated from attendance data.	2026-08-19 03:34:31.809109	5
6	2	2	2	0	0	80	20	2	0	2	0	Seeded summary generated from attendance data.	2026-08-19 03:34:31.809109	6
7	4	4	4	0	0	78	4.875	2	2	3	1	Seeded summary generated from attendance data.	2026-08-19 03:34:31.809109	7
8	4	4	4	0	0	196	12.25	2	2	3	1	Seeded summary generated from attendance data.	2026-08-19 03:34:31.809109	8
\.


--
-- Data for Name: qrcode; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.qrcode (qrcode_id, qrcode_token, time_expires, user_id, project_id) FROM stdin;
1	QR-1-CHECKIN	2026-07-31 21:00:00	18	1
2	QR-3-CHECKIN	2026-07-16 02:00:00	19	3
3	QR-8-CHECKIN	2026-08-09 01:00:00	21	8
\.


--
-- Data for Name: report; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.report (report_id, notes, status, date_created, user_id, project_id) FROM stdin;
1	Attendance report requires review.	PendingReview	2026-08-15 12:00:00	2	1
2	Project completion report submitted for assistant review.	AssistantReviewed	2026-08-15 12:00:00	21	7
3	Career readiness project report reviewed.	Reviewed	2026-08-15 12:00:00	15	8
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
-- Data for Name: staff; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.staff (staff_id, department) FROM stdin;
\.


--
-- Data for Name: student; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.student (student_id, student_number, year_of_study, qualification, department) FROM stdin;
26	202301002	Third	Bachelor of Commerce in Business Management	Department of Business Management
27	202301003	Second	Bachelor of Education	Department of Childhood Education
28	202301004	Fourth	BEng Civil Engineering	School of Civil Engineering and the Built Environment
29	202301005	Third	Bachelor of Information Management	Department of Information and Knowledge Management
30	202301006	Fourth	BCom Accountancy	Department of Accountancy
4	202301008	First	Bachelor of Education	Department of Childhood Education
5	202301009	Second	BCom Accountancy	Department of Accountancy
7	202301010	Fourth	BCom Accountancy	Department of Accountancy
8	202301011	First	BEng Civil Engineering	School of Civil Engineering and the Built Environment
9	202301012	Third	Bachelor of Information Management	Department of Information and Knowledge Management
11	202301013	Fourth	BCom Accountancy	Department of Accountancy
\.


--
-- Data for Name: suspension; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.suspension (suspension_id, start_date, end_date, is_active, user_id) FROM stdin;
1	2026-07-01	2026-07-15	f	32
2	2026-08-01	2026-08-07	f	35
3	2026-08-19	2026-08-20	t	5
\.


--
-- Data for Name: unregistered_volunteer; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.unregistered_volunteer (id, email, name, surname, campus_id, hours) FROM stdin;
1	walkin1@example.org	Sam	Mokoena	1	6
2	walkin2@example.org	Lebo	Ndlovu	2	3.5
3	walkin3@example.org	Chris	Naidoo	3	8
4	walkin4@example.org	Masego	Dube	4	4
\.


--
-- Data for Name: user; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."user" (user_id, first_name, last_name, email, phone, id_number, password, date_created, campus_id, is_active, is_suspended, role, faculty_id) FROM stdin;
1	Fiona	Director	director@uj.ac.za	+27821000001	STF10001	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2026-01-05	1	t	f	director	\N
2	Naledi	Dlamini	apk.spec1@uj.ac.za	+27821000002	STF10002	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2026-01-16	1	t	f	specialist	1
6	Sipho	Mokoena	apb.spec1@uj.ac.za	+27821000006	STF10006	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2026-02-05	2	t	f	specialist	4
10	Precious	Mahlangu	dfc.spec1@uj.ac.za	+27821000010	STF10010	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2026-02-25	3	t	f	specialist	8
14	Lindiwe	Zulu	swc.spec1@uj.ac.za	+27821000014	STF10014	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2026-03-15	4	t	f	specialist	8
19	Precious	Mahlangu	apb.assistant@uj.ac.za	+27822000002	STF20002	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2026-03-06	2	t	f	assistant	4
20	Mpho	Molefe	dfc.assistant@uj.ac.za	+27822000003	STF20003	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2026-03-07	3	t	f	assistant	8
21	Refilwe	Mokoena	swc.assistant@uj.ac.za	+27822000004	STF20004	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2026-03-07	4	t	f	assistant	8
22	Zanele	Khumalo	staff@uj.ac.za	+27823000001	STF30001	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2026-03-01	1	t	f	staff	1
23	Brian	Pillay	staff.apb@uj.ac.za	+27823000002	STF30002	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2026-03-02	2	t	f	staff	4
24	Nomsa	Dlamini	staff.dfc@uj.ac.za	+27823000003	STF30003	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2026-03-03	3	t	f	staff	8
25	Thabo	Molefe	staff.swc@uj.ac.za	+27823000004	STF30004	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2026-03-04	4	t	f	staff	8
26	Dean	Adams	dean.adams@student.uj.ac.za	+27821000019	202301002	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2023-04-02	2	t	f	volunteer	2
28	Chantelle	Naidoo	chantelle.naidoo@student.uj.ac.za	+27821000021	202301004	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2023-04-04	4	t	f	volunteer	4
29	Katlego	Mabaso	katlego.mabaso@student.uj.ac.za	+27821000022	202301005	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2023-04-05	1	t	f	volunteer	2
30	Werner	Steyn	werner.steyn@student.uj.ac.za	+27821000023	202301006	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2023-04-06	2	t	f	volunteer	1
33	Palesa	Moloi	palesa.moloi@gmail.com	+27821000026	201801009	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2023-04-09	1	t	f	volunteer	1
34	Riaan	Coetzee	riaan.coetzee@gmail.com	+27821000027	201801010	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2023-04-10	2	t	f	volunteer	2
15	Keitumetse	Molefe	swc.vol2@uj.ac.za	+27821000015	STF10015	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2026-03-20	4	t	f	volunteer	8
17	Zanele	Khumalo	swc.vol4@uj.ac.za	+27821000017	STF10017	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2026-03-30	4	t	f	volunteer	7
31	Nomvula	Cele	nomvula.cele@gmail.com	+27821000024	201801007	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2023-04-07	3	t	t	volunteer	3
5	Farai	Chikwara	apk.vol4@uj.ac.za	+27821000005	STF10005	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2026-02-01	1	t	t	volunteer	3
9	Johan	Botha	apb.vol4@uj.ac.za	+27821000009	STF10009	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2026-02-20	2	t	f	volunteer	7
7	Thabiso	Nkosi	apb.vol2@uj.ac.za	+27821000007	STF10007	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2026-02-10	2	t	f	volunteer	5
4	Amanda	Merwe	apk.vol3@uj.ac.za	+27821000004	STF10004	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2026-01-25	1	t	f	volunteer	2
12	Amanda	Ndlovu	dfc.vol3@uj.ac.za	+27821000012	STF10012	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2026-03-05	3	t	f	volunteer	9
8	Lerato	Molefe	apb.vol3@uj.ac.za	+27821000008	STF10008	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2026-02-15	2	t	f	volunteer	6
16	David	Mthembu	swc.vol3@uj.ac.za	+27821000016	STF10016	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2026-03-25	4	t	f	volunteer	7
13	Musa	Dube	dfc.vol4@uj.ac.za	+27821000013	STF10013	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2026-03-10	3	t	f	volunteer	9
11	Kagiso	Maseko	dfc.vol2@uj.ac.za	+27821000011	STF10011	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2026-03-01	3	t	f	volunteer	9
36	Grant	Pillay	grant.pillay@gmail.com	+27821000029	201801012	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2023-04-12	4	f	f	volunteer	4
27	Mpho	Letsoalo	mpho.letsoalo@student.uj.ac.za	+27821000020	202301003	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2023-04-03	3	f	f	volunteer	3
18	Johan	Botha	apk.vol2@uj.ac.za	+27822000001	STF20001	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2026-03-06	1	t	f	volunteer	1
3	Siyabonga	Mokoena	apk.assistant@uj.ac.za	+27821000003	STF10003	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2026-01-20	1	t	f	assistant	1
35	Thandeka	Ngwenya	thandeka.ngwenya@gmail.com	+27821000028	201801011	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2023-04-11	1	t	t	volunteer	3
32	Brandon	Smith	brandon.smith@gmail.com	+27821000025	201801008	f80c0cdc6d9c62253cd6a76ba2d5ef5d9b13e9	2023-04-08	1	f	f	volunteer	4
\.


--
-- Data for Name: user_demographics; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.user_demographics (user_id, geo_area, home_language, gender, is_international) FROM stdin;
26	Alexandra	Sepedi	Male	t
27	Soweto	isiZulu	Female	f
28	Tembisa	Setswana	Female	f
29	Roodepoort	English	Male	f
30	Midrand	Sesotho	Female	f
31	Alexandra	isiXhosa	Female	f
32	Soweto	Sepedi	Male	f
33	Tembisa	isiZulu	Female	t
34	Roodepoort	Setswana	Female	f
35	Midrand	English	Male	f
36	Alexandra	Sesotho	Female	f
3	Alex	Sepedi	Male	f
4	Motheo	Sesotho	Male	f
5	Midrand	isiXhosa	Female	f
7	AuklandPark	Setswana	Female	f
8	Soweto	isiZulu	Male	f
9	Roodepoort	English	Male	t
11	Tembisa	English	Female	t
12	Alexandra	Sesotho	Female	f
\.


--
-- Data for Name: volunteer; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.volunteer (volunteer_id, volunteer_type, is_completed, recruiter_id, hours) FROM stdin;
26	Student	t	\N	114
27	Student	f	\N	32
28	Student	f	\N	27.5
29	Student	t	\N	103
30	Student	f	\N	31
31	Alumni	f	\N	10
32	Alumni	f	\N	15
33	Alumni	f	\N	12
34	Alumni	f	\N	7
35	Alumni	f	\N	31
36	Alumni	f	\N	0
4	Student	t	\N	100
5	Student	f	\N	80
7	Student	f	\N	55
8	Student	f	\N	58
9	Student	f	\N	21
11	Student	f	\N	68
12	Alumni	t	\N	100
3	Alumni	t	\N	100
18	Student	f	\N	79
\.


--
-- Data for Name: volunteer_sdg; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.volunteer_sdg (volunteer_id, sdg_id) FROM stdin;
26	1
26	10
27	4
27	10
28	3
28	10
29	4
29	9
30	8
30	12
31	1
31	10
32	8
32	9
33	8
33	12
34	8
34	10
35	3
35	10
36	4
36	16
3	1
4	2
5	2
7	3
8	4
9	1
11	5
12	6
3	2
4	3
5	3
7	4
8	3
9	2
11	2
12	1
\.


--
-- Name: User_user_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."User_user_id_seq"', 36, true);


--
-- Name: attendance_attendance_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.attendance_attendance_id_seq', 8, true);


--
-- Name: attendance_entry_attendance_entry_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.attendance_entry_attendance_entry_id_seq', 29, true);


--
-- Name: campus_campus_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.campus_campus_id_seq', 4, true);


--
-- Name: certificate_certificate_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.certificate_certificate_id_seq', 3, true);


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

SELECT pg_catalog.setval('public.ngo_application_application_id_seq', 5, true);


--
-- Name: ngo_partnership_partnership_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.ngo_partnership_partnership_id_seq', 3, true);


--
-- Name: notification_notification_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.notification_notification_id_seq', 7, true);


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

SELECT pg_catalog.setval('public.project_project_id_seq', 10, true);


--
-- Name: project_summary_summary_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.project_summary_summary_id_seq', 8, true);


--
-- Name: qrcode_qrcode_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.qrcode_qrcode_id_seq', 3, true);


--
-- Name: report_report_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.report_report_id_seq', 3, true);


--
-- Name: sdg_sdg_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.sdg_sdg_id_seq', 17, true);


--
-- Name: suspension_suspension_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.suspension_suspension_id_seq', 3, true);


--
-- Name: unregistered_volunteer_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.unregistered_volunteer_id_seq', 4, true);


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



