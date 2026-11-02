import http from 'k6/http';
import { check, sleep } from 'k6';

const BASE_URL = __ENV.BASE_URL || 'http://localhost:5005';

export const options = {
  vus: 5,
  duration: '30s',
  thresholds: {
    http_req_failed: ['rate<0.05'],
    http_req_duration: ['p(95)<2000'],
    checks: ['rate>0.95'],
  },
};

export default function () {
  const homeRes = http.get(`${BASE_URL}/`, {
    tags: { name: 'HomePage' },
  });

  check(homeRes, {
    'home page returns 200': (r) => r.status === 200,
  });

  const volunteerRes = http.get(`${BASE_URL}/Home/Volunteer`, {
    tags: { name: 'VolunteerPage' },
  });

  check(volunteerRes, {
    'volunteer page returns 200': (r) => r.status === 200,
  });

  sleep(1);
}
