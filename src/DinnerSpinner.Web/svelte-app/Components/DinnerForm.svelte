<script>
  import { createEventDispatcher } from "svelte";

  const dispatch = createEventDispatcher();

  function handleUpsert(event) {
    const payload = {
      name: name,
      ingredients: ingredients.split("\n"),
    };

    return fetch(`/api/spinner/${id}/dinners`, {
      method: "POST",
      body: JSON.stringify(payload),
      headers: { "content-type": "application/json" },
    })
      .then((response) => response.json())
      .then((d) => {
        dispatch("append", {
          spinner: d,
        });
      });
  }

  export let name;
  export let id;
  let ingredients = "";

  $: displayName = name || "Add a new dinner";
</script>

<div>
  <h1>{displayName}</h1>
  <input bind:value={name} />
  <textarea bind:value={ingredients} />
  <button on:click={handleUpsert}>Save</button>
</div>
