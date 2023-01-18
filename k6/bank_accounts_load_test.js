import http from 'k6/http';
import { sleep } from 'k6';


export const options = {
    vus: 10,
    iterations: 100
}

export default function () {

    const params = {
        headers: {
            'Content-Type': 'application/json',
            'x-api-version': '2',
            'ClientId': 'Twitter'
        },
    };

    sleep(0.3)
    http.get('http://localhost:5001/bank-accounts', params);
}