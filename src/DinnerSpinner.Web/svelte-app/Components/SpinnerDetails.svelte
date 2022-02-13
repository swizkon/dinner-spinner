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
	console.log("event", event);
	spinner = event.detail.spinner;
	}

	function handleDelete(event) {
		console.log("handleDelete", event);
		// spinner = event.detail.spinner;
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
    <DinnerItem
      {dinner}
      on:delete={handleDelete}
    />
  {/each}

  <hr />
  <DinnerForm {id} on:append={handleAppend} />
</div>
