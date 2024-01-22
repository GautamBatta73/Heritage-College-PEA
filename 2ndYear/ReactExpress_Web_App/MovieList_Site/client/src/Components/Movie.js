import React, { useState, useEffect, useContext } from 'react';
import { IdContext } from './IdContext';
import Actor from './Actor';
import Genre from './Genre';

export default function Movie() {
    const id = useContext(IdContext);
    const [movie, setMovie] = useState([])

    useEffect(() => {
        const fetchMovie = async () => {
            setMovie(await fetch(`/movies/${id}`)
                .then(resp => resp.json())
                .then(data => data));
        }
        fetchMovie();
    }, [id]);
    return (
        <>
            <br />
            &emsp;Runtime: {`${movie.Runtime} mins`}
            <br />
            &emsp;Revenue: {`$${movie.Revenue} Million`}
            <Actor />
            <Genre />
        </>
    )
}