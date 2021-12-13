<script>
  import { onMount } from "svelte";
  import { createEventDispatcher } from "svelte";

  const dispatch = createEventDispatcher();

  function getDetails() {
    return fetch(`/api/spinner/${id}`)
      .then((response) => response.json())
      .then((s) => {
        spinner = s;
        console.log("söpinner", spinner);
      });
  }

  onMount(async () => {
    console.log("spinner", spinner);
    await getDetails();
    console.log("spinner", spinner);
  });

  let spinner = null;
  $: title = spinner && spinner.name || "Loading...";

  export let id;
</script>

<div>
  <h1>{title} <small>({id})</small></h1>
</div>
