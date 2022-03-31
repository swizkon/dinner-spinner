<script>
  import { onMount } from "svelte";
  import { apiData, spinners } from "./data/stores.js";
  import { Router, Link, Route } from "svelte-routing";

  import SpinnerList from "./Components/SpinnerList.svelte";

  import SpinnerDetails from "./Components/SpinnerDetails.svelte";

  onMount(async () => {
    fetch("/api/spinner")
      .then((response) => response.json())
      .then((data) => {
        apiData.set(data);
      })
      .catch((error) => {
        console.log(error);
        return [];
      });
  });

  function handleDelete(event) {
    var state = $spinners.filter((s) => s.id !== event.detail.spinnerId);
    apiData.set(state);

    return fetch(`/api/spinner/${event.detail.spinnerId}`, {
      method: "DELETE",
    }).then((response) => response.json());
  }

  function handleAppend(event) {
    console.log(event.detail.spinner);
    var state = $spinners.concat([event.detail.spinner]);
    apiData.set(state);
  }

  export let name;
  export let url = "";
</script>

<main>
  <h2>Hello {name}!</h2>

  <Router {url}>
    <nav>
      <Link to="/">Home</Link>
    </nav>
    <div>
      <Route path="spinner/:id" let:params>
        <SpinnerDetails id={params.id} />
      </Route>
      <Route path="/" component={SpinnerList} />
    </div>
  </Router>
</main>

<style>
  main {
    text-align: center;
    padding: 1em;
    max-width: 240px;
    margin: 0 auto;
  }
  /*
  h1 {
    color: #ff3e00;
    text-transform: uppercase;
    font-size: 4em;
    font-weight: 100;
    text-shadow: 3px 3px 0em #000;
  }  */

  @media (min-width: 640px) {
    main {
      max-width: none;
    }
  }
</style>
