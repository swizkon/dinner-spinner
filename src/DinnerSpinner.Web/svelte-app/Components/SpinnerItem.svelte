<script>
	import { onMount } from "svelte";
	import { createEventDispatcher } from 'svelte';

	const dispatch = createEventDispatcher();

	function deleteMe() {
		dispatch('delete', {
			spinnerId: id
		});
	}
	
onMount(async () => {
  console.log('spinner', spinner)
});

	export let name;
	export let id;
	export let spinner;
</script>

<div>
	<h1>{name} <small>({id})</small></h1>
	<button on:click={deleteMe}>
		Delete
	</button>
</div>