import React, { useState, useEffect, useContext } from 'react';
import { IdContext } from './IdContext';

export default function Genre() {
    const id = useContext(IdContext);
    const [movie, setMovie] = useState([]);

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
            &emsp;Genre:&nbsp;
            {movie.Genre && movie.Genre.length ? (
                <>
                    {movie.Genre.map((genre) => (
                        <>
                            <br />
                            &emsp;&emsp;{genre}
                        </>
                    ))}
                </>
            ) : (
                <>No genre found.</>
            )}
        </>
    )
}
