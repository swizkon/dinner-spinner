<script>
	import { createEventDispatcher } from 'svelte';

	const dispatch = createEventDispatcher();

	function deleteMe() {
		dispatch('delete', {
			spinnerId: id
		});
	}
	export let name;
	export let id;
</script>

<div>
	<h1>{name} <small>({id})</small></h1>
	<button on:click={deleteMe}>
		Delete
	</button>
</div>