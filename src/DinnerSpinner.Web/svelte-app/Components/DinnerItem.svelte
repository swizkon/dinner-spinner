<script>
  import { onMount } from "svelte";
  import { createEventDispatcher } from "svelte";

  const dispatch = createEventDispatcher();

  function deleteMe() {
    console.log("dinner item deleteMe", dinner);
    dispatch("delete", {
      dinnerId: dinner.id,
    });
  }

  function toggleEdit() {

    if (editMode && dinner.name != name) { 
      
      dispatch("renameDinner", {
        dinnerId: dinner.id,
        name: name,
      });
    }
    
    editMode = !editMode;
  }

  onMount(async () => {
    console.log("dinner item", dinner);
    name = dinner.name;
  });

  export let dinner;

  let name;
  let editMode = false
  $: debug = " editmode: " + editMode;
</script>

<div>

  {#if editMode}
  <input on:blur={toggleEdit} bind:value={name} />
  {:else}
  <h2 on:click={toggleEdit}>{name}</h2>
  {/if}
  
  <hr/>
  <button on:click={deleteMe}> Delete </button>
  <em>{debug}</em>
</div>
