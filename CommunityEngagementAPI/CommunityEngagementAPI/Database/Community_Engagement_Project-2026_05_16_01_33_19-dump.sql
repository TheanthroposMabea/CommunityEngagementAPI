--
-- PostgreSQL database dump
--

\restrict eohsYvISf0OcTf6pW27g2fbzSf7RDSflBuC9mAXU3uyvPh1jUCWkSKS9dGhagnK

-- Dumped from database version 18.3
-- Dumped by pg_dump version 18.3

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

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
    user_role_id integer CONSTRAINT "User_user_role_id_not_null" NOT NULL,
    is_active integer NOT NULL
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
-- Name: alumni; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.alumni (
    alumni_id integer NOT NULL,
    year_graduated character varying NOT NULL
);


ALTER TABLE public.alumni OWNER TO postgres;

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
-- Name: faculty; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.faculty (
    faculty_id integer NOT NULL,
    name character varying NOT NULL,
    campus_id integer NOT NULL
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
-- Name: role; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.role (
    role_id integer NOT NULL,
    name character varying NOT NULL,
    description character varying,
    is_active boolean NOT NULL
);


ALTER TABLE public.role OWNER TO postgres;

--
-- Name: role_role_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.role ALTER COLUMN role_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.role_role_id_seq
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
    name character varying NOT NULL,
    icon character varying
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
    staff_number character varying NOT NULL,
    department character varying NOT NULL
);


ALTER TABLE public.staff OWNER TO postgres;

--
-- Name: student; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.student (
    student_id integer NOT NULL,
    student_number character varying NOT NULL,
    year_of_study character varying NOT NULL,
    qualification character varying NOT NULL,
    faculty_id integer NOT NULL
);


ALTER TABLE public.student OWNER TO postgres;

--
-- Name: user_sdg; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.user_sdg (
    user_id integer NOT NULL,
    sdg_id integer NOT NULL
);


ALTER TABLE public.user_sdg OWNER TO postgres;

--
-- Name: userdemographics; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.userdemographics (
    user_id integer NOT NULL,
    geo_area character varying NOT NULL,
    home_language character varying NOT NULL,
    gender character varying NOT NULL,
    isinternational boolean NOT NULL
);


ALTER TABLE public.userdemographics OWNER TO postgres;

--
-- Name: volunteer; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.volunteer (
    volunteer_id integer NOT NULL,
    volunteer_type character varying NOT NULL,
    issuspended boolean NOT NULL,
    recruiter_id integer
);


ALTER TABLE public.volunteer OWNER TO postgres;

--
-- Data for Name: alumni; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.alumni (alumni_id, year_graduated) FROM stdin;
\.


--
-- Data for Name: campus; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.campus (campus_id, code, name) FROM stdin;
1	0	Auckland Park
\.


--
-- Data for Name: faculty; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.faculty (faculty_id, name, campus_id) FROM stdin;
1	Science	1
\.


--
-- Data for Name: role; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.role (role_id, name, description, is_active) FROM stdin;
1	Student	\N	t
\.


--
-- Data for Name: sdg; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.sdg (sdg_id, name, icon) FROM stdin;
\.


--
-- Data for Name: staff; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.staff (staff_id, staff_number, department) FROM stdin;
\.


--
-- Data for Name: student; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.student (student_id, student_number, year_of_study, qualification, faculty_id) FROM stdin;
\.


--
-- Data for Name: user; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."user" (user_id, first_name, last_name, email, phone, id_number, password, date_created, campus_id, user_role_id, is_active) FROM stdin;
1	Rapelang Mabea	Rapelang Mabea	220091825@student.uj.ac.za	0810793698	0123456789123	180cf8846d8a2d5b0eba6bf9651d5decb0cc65	2026-05-16	1	1	1
\.


--
-- Data for Name: user_sdg; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.user_sdg (user_id, sdg_id) FROM stdin;
\.


--
-- Data for Name: userdemographics; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.userdemographics (user_id, geo_area, home_language, gender, isinternational) FROM stdin;
\.


--
-- Data for Name: volunteer; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.volunteer (volunteer_id, volunteer_type, issuspended, recruiter_id) FROM stdin;
\.


--
-- Name: User_user_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."User_user_id_seq"', 1, true);


--
-- Name: campus_campus_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.campus_campus_id_seq', 1, true);


--
-- Name: faculty_faculty_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.faculty_faculty_id_seq', 1, true);


--
-- Name: role_role_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.role_role_id_seq', 1, true);


--
-- Name: sdg_sdg_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.sdg_sdg_id_seq', 1, false);


--
-- Name: alumni alumni_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.alumni
    ADD CONSTRAINT alumni_pk PRIMARY KEY (alumni_id);


--
-- Name: campus campus_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.campus
    ADD CONSTRAINT campus_pk PRIMARY KEY (campus_id);


--
-- Name: faculty faculty_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.faculty
    ADD CONSTRAINT faculty_pk PRIMARY KEY (faculty_id);


--
-- Name: role role_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.role
    ADD CONSTRAINT role_pk PRIMARY KEY (role_id);


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
-- Name: user user_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."user"
    ADD CONSTRAINT user_pk PRIMARY KEY (user_id);


--
-- Name: user_sdg user_sdg_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_sdg
    ADD CONSTRAINT user_sdg_pkey PRIMARY KEY (user_id, sdg_id);


--
-- Name: userdemographics userdemographics_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.userdemographics
    ADD CONSTRAINT userdemographics_pk PRIMARY KEY (user_id);


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
-- Name: user User_user_role_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."user"
    ADD CONSTRAINT "User_user_role_id_fkey" FOREIGN KEY (user_role_id) REFERENCES public.role(role_id);


--
-- Name: alumni alumni___fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.alumni
    ADD CONSTRAINT alumni___fk FOREIGN KEY (alumni_id) REFERENCES public.volunteer(volunteer_id);


--
-- Name: faculty faculty_campus_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.faculty
    ADD CONSTRAINT faculty_campus_id_fkey FOREIGN KEY (campus_id) REFERENCES public.campus(campus_id);


--
-- Name: staff staff___fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.staff
    ADD CONSTRAINT staff___fk FOREIGN KEY (staff_id) REFERENCES public.volunteer(volunteer_id);


--
-- Name: student student___fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.student
    ADD CONSTRAINT student___fk FOREIGN KEY (faculty_id) REFERENCES public.faculty(faculty_id);


--
-- Name: student student_student_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.student
    ADD CONSTRAINT student_student_id_fkey FOREIGN KEY (student_id) REFERENCES public.volunteer(volunteer_id);


--
-- Name: user_sdg user_sdg_sdg_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_sdg
    ADD CONSTRAINT user_sdg_sdg_id_fkey FOREIGN KEY (sdg_id) REFERENCES public.sdg(sdg_id);


--
-- Name: user_sdg user_sdg_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_sdg
    ADD CONSTRAINT user_sdg_user_id_fkey FOREIGN KEY (user_id) REFERENCES public."user"(user_id);


--
-- Name: userdemographics userdemographics_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.userdemographics
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

\unrestrict eohsYvISf0OcTf6pW27g2fbzSf7RDSflBuC9mAXU3uyvPh1jUCWkSKS9dGhagnK

