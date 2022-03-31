<script>
  import { onMount } from "svelte";
  import { createEventDispatcher } from "svelte";

  import DinnerItem from "./DinnerItem.svelte";
  import DinnerForm from "./DinnerForm.svelte";

  const dispatch = createEventDispatcher();

  function getDetails() {
    return fetch(`/api/spinner/${id}`)
      .then((response) => response.json())
      .then((s) => {
        spinner = s;
      });
  }

  onMount(async () => {
    await getDetails();
  });

  function handleAppend(event) {
    spinner = event.detail.spinner;
  }
  
  function handleRename(event) {
    console.log("handleRename", event);
    
    const payload = {
      name: event.detail.name
    };

    return fetch(`/api/spinner/${id}/dinners/${event.detail.dinnerId}`, {
      method: "PATCH",
      body: JSON.stringify(payload),
      headers: { "content-type": "application/json" },
    })
      .then((response) => response.json())
      .then((d) => {
        spinner = d;
      });
  }

  function handleDelete(event) {
    console.log("handleDelete", event);
    return fetch(`/api/spinner/${id}/dinners/${event.detail.dinnerId}`, {
      method: "DELETE",
      headers: { "content-type": "application/json" },
    })
      .then((response) => response.json())
      .then((d) => {
        spinner = d;
      });
  }

  let spinner = null;
  $: title = (spinner && spinner.name) || "Loading...";
  $: dinners = (spinner && spinner.dinners) || [];

  export let id;
</script>

<div>
  <h1>{title}</h1>
  <hr />

  {#each dinners as dinner}
    <DinnerItem {dinner} on:renameDinner={handleRename} on:delete={handleDelete} />
  {/each}

  <hr />
  <DinnerForm {id} on:append={handleAppend} />
</div>
