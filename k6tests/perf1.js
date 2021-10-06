import { check, sleep } from "k6";
import http from "k6/http";

// Export an options object to configure how k6 will behave during test execution.
//
// See https://docs.k6.io/docs/options
//
export let options = {

  // Either specify vus + duration or stages
  // vus: 20,
  // duration: "3m",

  // This stages configuration will ramp to 20 Virtual Users over a minute,
  // maintain those 20 concurrent users for 1 minute
  // then ramp down to 0 over a minute i.e. ramp-up pattern of "load"
  stages: [
    { duration: "1m", target: 20 },           // 1 new vu every 3 seconds
    { duration: "1m", target: 20 },
    { duration: "1m", target: 0 }             // 1 less vu every 3 seconds
  ],
  
  // set a threshold at 100 ms request duration for 95th percentile
  // request duration = time spent sending request, waiting for response, and receiving response
  // aka "response time"
  // the test will be marked as failed by threshold if the value is exceeded 
  // i.e. 95% of request durations should be < 100 ms
 	thresholds: {
    "http_req_duration": ["p(95) < 200"]
  },

  // Don't save the bodies of HTTP responses by default, for improved performance
  // Can be overwritten by setting the `responseType` option to `text` or `binary` for individual requests
  discardResponseBodies: true,

  ext: {
    loadimpact: {
      // Specify the distribution across load zones
      //
      // See https://docs.k6.io/docs/cloud-execution#section-cloud-execution-options
      //
      distribution: {
        loadZoneLabel1: { loadZone: "amazon:ie:dublin", percent: 100 },

        // Uncomment this and make sure percentage distribution adds up to 100 to use two load zones.
        // loadZoneLabel2: { loadZone: "amazon:us:ashburn", percent: 50 }
      }
    }
  }
};

// Export a default function - this defines the entry point for your VUs,
// similar to the main() function in many other languages.
export default function() {
  let res = http.get("http://gc-bmicalculator-ga-staging.azurewebsites.net");

  check(res, {
      "is status 200": (r) => r.status === 200
  });

// "think" for 3 seconds
  sleep(3);
}

// to run on Docker:
// docker pull loadimpact/k6
// docker run -i loadimpact/k6 run - <perf1.js