import http from 'k6/http';

export const options = {
    vus: 50,
    iterations: 1000
}

export default function () {

    const params = {
        headers: {
            'Content-Type': 'application/json',
            'x-api-version': '1'
        },
    };

    http.get('http://localhost:5001/bank-accounts', params);
}