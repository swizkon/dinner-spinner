<script>
	import { createEventDispatcher } from "svelte";

	const dispatch = createEventDispatcher();

	function handleUpsert(event) {
	const payload = {
	name: name,
	ownerEmail: "jonas@example.com",
	ownerName: "jonas",
	};

	return fetch("/api/spinner", {
	method: "POST",
	body: JSON.stringify(payload),
	headers: { "content-type": "application/json" },
	})
	.then((response) => response.json())
	.then((s) => {
	dispatch("append", {
	spinner: s,
	});
	});
	}

	let name;
	export let id;

	$: displayName = name || 'New spinner:'
</script>

<div>
  <h1>{displayName} <small>({id})</small></h1>
  <input bind:value={name} />
  <button on:click={handleUpsert}>Save</button>
</div>
