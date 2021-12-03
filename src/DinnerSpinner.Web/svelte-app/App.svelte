<script>
import { onMount } from "svelte";
import { apiData, spinners } from './data/stores.js';


import SpinnerItem from "./Components/SpinnerItem.svelte";

onMount(async () => {
  fetch("/api/spinner")
  .then(response => response.json())
  .then(data => {
    apiData.set(data);
  }).catch(error => {
    console.log(error);
    return [];
  });
});

function handleDelete(event) {
	var state = $spinners.filter(s => s.id !== event.detail.spinnerId);
    apiData.set(state);

	return fetch(`/api/spinner/${event.detail.spinnerId}`, {
        method: 'DELETE'
    }).then(response => response.json())
}

export let name;

</script>

<main>
	<h1>Hello {name}!</h1>
	<h2>Spinners:</h2>
	{#each $spinners as spinner}
		<SpinnerItem name={spinner.name} id={spinner.id} on:delete={handleDelete} />
	{/each}
	
	<hr/>
	<p>Visit the <a href="https://svelte.dev/tutorial">Svelte tutsfsfdsdforial</a> to learn how to build Svelte apps.</p>
</main>

<style>
	main {
		text-align: center;
		padding: 1em;
		max-width: 240px;
		margin: 0 auto;
	}

	h1 {
		color: #ff3e00;
		text-transform: uppercase;
		font-size: 4em;
		font-weight: 100;
	}

	@media (min-width: 640px) {
		main {
			max-width: none;
		}
	}
</style>